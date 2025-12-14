from __future__ import annotations

import json
import os
import re
import time
import unicodedata
from dataclasses import dataclass
from pathlib import Path
from typing import Any

import boto3


@dataclass(frozen=True)
class _Cell:
    row: int
    col: int
    row_span: int
    col_span: int
    text: str
    page: int
    bbox: dict[str, float]  # Textract BoundingBox: Left, Top, Width, Height


def handle_pdf(
    pdf_path: Path,
    *,
    json_root: str | Path = "json",
    s3_bucket: str | None = None,
    s3_prefix: str = "textract-input/",
    aws_region: str | None = None,
    poll_seconds: float = 1.5,
    max_poll_seconds: int = 600,
) -> None:
    """
    Extracts product names (column "Désignation") and weekly quantity sold
    (column "Qte" under "Sommaire hebdo") from a scanned PDF using AWS Textract.

    Writes JSON to:
      {json_root}/{pdf_parent_folder_name}/{pdf_stem}.json
    """
    pdf_path = Path(pdf_path)

    if not pdf_path.exists() or not pdf_path.is_file():
        raise ValueError(f"PDF path must exist and be a file: {pdf_path}")

    bucket = s3_bucket or os.getenv("TEXTRACT_S3_BUCKET")
    if not bucket:
        raise ValueError(
            "Missing S3 bucket. Pass s3_bucket=... or set TEXTRACT_S3_BUCKET."
        )

    prefix = s3_prefix or os.getenv("TEXTRACT_S3_PREFIX", "textract-input/")
    prefix = prefix.strip("/")
    key = f"{prefix}/{pdf_path.parent.name}/{pdf_path.name}"

    region = aws_region or os.getenv("AWS_REGION") or os.getenv("AWS_DEFAULT_REGION")

    s3 = boto3.client("s3", region_name=region)
    textract = boto3.client("textract", region_name=region)

    _upload_to_s3(s3, bucket=bucket, key=key, file_path=pdf_path)

    job_id = _start_textract_job(
        textract,
        bucket=bucket,
        key=key,
    )

    blocks = _poll_and_get_blocks(
        textract,
        job_id=job_id,
        poll_seconds=poll_seconds,
        max_poll_seconds=max_poll_seconds,
    )

    records = _extract_products_and_qty_from_blocks(blocks)

    out_dir = Path(json_root) / pdf_path.parent.name
    out_dir.mkdir(parents=True, exist_ok=True)
    out_path = out_dir / f"{pdf_path.stem}.json"

    with out_path.open("w", encoding="utf-8") as f:
        json.dump(records, f, ensure_ascii=False, indent=2)


def _upload_to_s3(s3: Any, *, bucket: str, key: str, file_path: Path) -> None:
    s3.upload_file(str(file_path), bucket, key)


def _start_textract_job(textract: Any, *, bucket: str, key: str) -> str:
    resp = textract.start_document_analysis(
        DocumentLocation={"S3Object": {"Bucket": bucket, "Name": key}},
        FeatureTypes=["TABLES"],
    )
    return resp["JobId"]


def _poll_and_get_blocks(
    textract: Any,
    *,
    job_id: str,
    poll_seconds: float,
    max_poll_seconds: int,
) -> list[dict[str, Any]]:
    deadline = time.time() + max_poll_seconds

    status = "IN_PROGRESS"
    while status in {"IN_PROGRESS"}:
        if time.time() > deadline:
            raise TimeoutError(
                f"Textract job {job_id} did not finish within {max_poll_seconds}s"
            )

        resp = textract.get_document_analysis(JobId=job_id, MaxResults=1000)
        status = resp["JobStatus"]

        if status == "SUCCEEDED":
            break
        if status in {"FAILED", "PARTIAL_SUCCESS"}:
            msg = resp.get("StatusMessage", "")
            raise RuntimeError(f"Textract job {job_id} ended with {status}. {msg}")

        time.sleep(poll_seconds)

    # Fetch all pages (pagination)
    blocks: list[dict[str, Any]] = []
    next_token: str | None = None

    while True:
        if next_token:
            resp = textract.get_document_analysis(
                JobId=job_id, MaxResults=1000, NextToken=next_token
            )
        else:
            resp = textract.get_document_analysis(JobId=job_id, MaxResults=1000)

        blocks.extend(resp.get("Blocks", []))
        next_token = resp.get("NextToken")
        if not next_token:
            break

    return blocks


def _extract_products_and_qty_from_blocks(
    blocks: list[dict[str, Any]],
) -> list[dict[str, Any]]:
    blocks_by_id = {b["Id"]: b for b in blocks}

    # Collect all TABLE blocks
    table_blocks = [b for b in blocks if b.get("BlockType") == "TABLE"]

    results: list[dict[str, Any]] = []

    for table in table_blocks:
        table_page = int(table.get("Page", 1))
        cells = _table_cells(table, blocks_by_id)
        if not cells:
            continue

        # Find the header row containing "Désignation"
        des_cell = _find_best_cell(cells, _is_designation)
        if not des_cell:
            continue

        header_row = des_cell.row
        des_col = des_cell.col

        # Find "Sommaire hebdo" in a row above the header row
        som_cell = _find_sommaire_cell_above(cells, header_row=header_row)
        summary_cols = _infer_summary_columns(
            cells, header_row=header_row, som_cell=som_cell
        )

        qty_cell = _find_qty_cell(
            cells,
            header_row=header_row,
            des_col=des_col,
            summary_cols=summary_cols,
        )
        if not qty_cell:
            # If we found Désignation but not Qte, skip this table.
            continue

        qty_col = qty_cell.col

        # Extract downwards until Désignation is empty.
        cell_map: dict[tuple[int, int], _Cell] = {(c.row, c.col): c for c in cells}
        max_row = max(c.row for c in cells)

        for r in range(header_row + 1, max_row + 1):
            name_cell = cell_map.get((r, des_col))
            if not name_cell or _is_empty(name_cell.text):
                break

            qty_val = cell_map.get((r, qty_col))
            qty = _parse_quantity(qty_val.text if qty_val else "")

            results.append(
                {
                    "page": table_page,
                    "product": name_cell.text.strip(),
                    "quantity": qty,
                }
            )

    return results


def _table_cells(
    table_block: dict[str, Any],
    blocks_by_id: dict[str, dict[str, Any]],
) -> list[_Cell]:
    cells: list[_Cell] = []
    rels = table_block.get("Relationships", [])
    child_ids: list[str] = []
    for rel in rels:
        if rel.get("Type") == "CHILD":
            child_ids.extend(rel.get("Ids", []))

    for cid in child_ids:
        b = blocks_by_id.get(cid)
        if not b or b.get("BlockType") != "CELL":
            continue

        text = _cell_text(b, blocks_by_id)
        geom = b.get("Geometry", {}) or {}
        bbox = geom.get("BoundingBox", {}) or {}

        cells.append(
            _Cell(
                row=int(b.get("RowIndex", 0)),
                col=int(b.get("ColumnIndex", 0)),
                row_span=int(b.get("RowSpan", 1)),
                col_span=int(b.get("ColumnSpan", 1)),
                text=text,
                page=int(b.get("Page", 1)),
                bbox={
                    "Left": float(bbox.get("Left", 0.0)),
                    "Top": float(bbox.get("Top", 0.0)),
                    "Width": float(bbox.get("Width", 0.0)),
                    "Height": float(bbox.get("Height", 0.0)),
                },
            )
        )

    return cells


def _cell_text(
    cell_block: dict[str, Any],
    blocks_by_id: dict[str, dict[str, Any]],
) -> str:
    parts: list[str] = []
    for rel in cell_block.get("Relationships", []):
        if rel.get("Type") != "CHILD":
            continue
        for cid in rel.get("Ids", []):
            child = blocks_by_id.get(cid)
            if not child:
                continue
            bt = child.get("BlockType")
            if bt == "WORD":
                parts.append(child.get("Text", ""))
            elif bt == "SELECTION_ELEMENT":
                if child.get("SelectionStatus") == "SELECTED":
                    parts.append("X")
    return " ".join(p for p in parts if p).strip()


def _norm(s: str) -> str:
    s = s or ""
    s = s.replace("\u00a0", " ").strip().lower()
    s = unicodedata.normalize("NFKD", s)
    s = "".join(ch for ch in s if not unicodedata.combining(ch))
    s = re.sub(r"\s+", " ", s)
    return s


def _is_empty(s: str) -> bool:
    return _norm(s) == ""


def _is_designation(s: str) -> bool:
    t = _norm(s)
    return t == "designation" or "designation" in t


def _is_qte(s: str) -> bool:
    t = _norm(s)
    return t in {"qte", "qté"} or t.startswith("qte")


def _is_sommaire_hebdo(s: str) -> bool:
    t = _norm(s)
    return "sommaire" in t and "hebdo" in t


def _find_best_cell(cells: list[_Cell], predicate) -> _Cell | None:
    matches = [c for c in cells if predicate(c.text)]
    if not matches:
        return None
    # Prefer top-most, then left-most.
    return sorted(matches, key=lambda c: (c.row, c.col))[0]


def _find_sommaire_cell_above(
    cells: list[_Cell],
    *,
    header_row: int,
) -> _Cell | None:
    candidates = [c for c in cells if c.row < header_row and _is_sommaire_hebdo(c.text)]
    if not candidates:
        return None
    # Prefer the closest row above the header; if tie, prefer right-most (summary is on right)
    return sorted(candidates, key=lambda c: (header_row - c.row, -c.col))[0]


def _infer_summary_columns(
    cells: list[_Cell],
    *,
    header_row: int,
    som_cell: _Cell | None,
) -> set[int] | None:
    """
    Attempts to infer which header_row columns fall under the "Sommaire hebdo"
    group header.

    Returns a set of column indices or None if not inferable.
    """
    if not som_cell:
        return None

    # Best case: Textract gives a ColumnSpan for the merged group header.
    if som_cell.col_span and som_cell.col_span > 1:
        start = som_cell.col
        end = som_cell.col + som_cell.col_span - 1
        return set(range(start, end + 1))

    # Fallback: use geometry overlap (header cell centers under sommaire bbox).
    left = som_cell.bbox["Left"]
    right = som_cell.bbox["Left"] + som_cell.bbox["Width"]
    header_cells = [c for c in cells if c.row == header_row]

    cols: set[int] = set()
    for c in header_cells:
        cx = c.bbox["Left"] + (c.bbox["Width"] / 2.0)
        if left <= cx <= right:
            cols.add(c.col)

    return cols or None


def _find_qty_cell(
    cells: list[_Cell],
    *,
    header_row: int,
    des_col: int,
    summary_cols: set[int] | None,
) -> _Cell | None:
    header_cells = [c for c in cells if c.row == header_row]
    qtes = [c for c in header_cells if _is_qte(c.text)]
    if not qtes:
        return None

    # Prefer Qte that is under Sommaire hebdo.
    if summary_cols:
        under_summary = [c for c in qtes if c.col in summary_cols]
        if under_summary:
            return sorted(under_summary, key=lambda c: c.col)[0]

    # Fallback heuristic: weekly summary Qte is usually the right-most Qte on that row.
    # Also ensure it's to the right of "Désignation".
    right_side = [c for c in qtes if c.col > des_col]
    if right_side:
        return sorted(right_side, key=lambda c: c.col)[-1]

    return sorted(qtes, key=lambda c: c.col)[-1]


def _parse_quantity(v: str) -> float | None:
    t = v.strip()
    if _is_empty(t):
        return None

    # Normalize: "1 234" -> "1234", "12,5" -> "12.5"
    t = t.replace("\u00a0", " ")
    t = t.replace(" ", "")
    t = t.replace(",", ".")
    m = re.search(r"-?\d+(?:\.\d+)?", t)
    if not m:
        return None

    x = float(m.group(0))
    return int(x) if x.is_integer() else x
