from __future__ import annotations

import re
import time
import unicodedata
from dataclasses import dataclass
from pathlib import Path
from typing import Any

import boto3

SALES_TOTAL_KEY = "__weekly_sales_total__"

def handle_pdf(
    pdf_path: Path,
    *,
    s3_bucket: str = "vincentliu-bucket-demo",
    s3_key: str | None = None,
    textract_client=None,
    s3_client=None,
    job_poll_seconds: float = 2.0,
    job_timeout_seconds: float = 15 * 60,
) -> dict[str, float] | None:
    """
    Extract products + weekly quantity sold from a scanned PDF (one page = one sheet)
    using Amazon Textract table analysis.

    Heuristics implemented (based on your constraints):
      - Do NOT rely on merged cells. We ignore MERGED_CELL blocks and only use CELL.
      - Find the header row by locating a cell equal to "Désignation" (accent-insensitive).
      - Quantity column is chosen as:
          1) the "Qte" cell immediately left of "Poids" in the same header row, if possible
             (this targets the Sommaire hebdo section),
          2) otherwise, the rightmost standalone "Qte" cell in the header row.
      - Read rows until the "Désignation" cell is empty.

    Output JSON:
      json/<pdf_parent_folder_name>/<pdf_stem>.json
      Includes a reserved key for weekly sales total: __weekly_sales_total__

    Notes:
      - Textract async APIs require the PDF to be in S3.
      - Requires IAM permissions for S3 put/get and Textract Start/GetDocumentAnalysis.
    """
    pdf_path = Path(pdf_path)

    if textract_client is None:
        textract_client = boto3.client("textract", region_name="us-east-1")
    if s3_client is None:
        s3_client = boto3.client("s3", region_name="us-east-1")

    if s3_key is None:
        # Default: mirror local parent folder + filename into S3
        s3_key = f"textract-input/{pdf_path.parent.name}/{pdf_path.name}"

    _upload_to_s3(s3_client, pdf_path=pdf_path, bucket=s3_bucket, key=s3_key)

    job_id = _start_textract_tables_job(
        textract_client,
        bucket=s3_bucket,
        key=s3_key,
    )

    blocks = _wait_and_collect_textract_blocks(
        textract_client,
        job_id=job_id,
        poll_seconds=job_poll_seconds,
        timeout_seconds=job_timeout_seconds,
    )

    records = _extract_products_and_qty_from_blocks(blocks)

    return records


# -----------------------------
# Textract job helpers
# -----------------------------


def _upload_to_s3(s3_client, *, pdf_path: Path, bucket: str, key: str) -> None:
    if not pdf_path.exists() or not pdf_path.is_file():
        raise ValueError(f"PDF path must exist and be a file: {pdf_path}")

    s3_client.upload_file(
        Filename=str(pdf_path),
        Bucket=bucket,
        Key=key,
    )


def _start_textract_tables_job(textract_client, *, bucket: str, key: str) -> str:
    resp = textract_client.start_document_analysis(
        DocumentLocation={"S3Object": {"Bucket": bucket, "Name": key}},
        FeatureTypes=["TABLES"],
    )
    return resp["JobId"]


def _wait_and_collect_textract_blocks(
    textract_client,
    *,
    job_id: str,
    poll_seconds: float,
    timeout_seconds: float,
) -> list[dict[str, Any]]:
    deadline = time.time() + timeout_seconds
    next_token: str | None = None
    all_blocks: list[dict[str, Any]] = []
    status = "IN_PROGRESS"

    # Wait for job completion (and fetch the first page of results)
    while True:
        if time.time() > deadline:
            raise TimeoutError(
                f"Textract job timed out after {timeout_seconds}s (JobId={job_id})"
            )

        kwargs = {"JobId": job_id}
        if next_token:
            kwargs["NextToken"] = next_token

        resp = textract_client.get_document_analysis(**kwargs)
        status = resp["JobStatus"]

        if status in {"IN_PROGRESS"}:
            time.sleep(poll_seconds)
            continue

        if status not in {"SUCCEEDED"}:
            msg = resp.get("StatusMessage", "")
            raise RuntimeError(
                f"Textract job failed (JobId={job_id}, Status={status}): {msg}"
            )

        # Job succeeded: collect this page and paginate
        all_blocks.extend(resp.get("Blocks", []))
        next_token = resp.get("NextToken")

        while next_token:
            resp = textract_client.get_document_analysis(
                JobId=job_id,
                NextToken=next_token,
            )
            all_blocks.extend(resp.get("Blocks", []))
            next_token = resp.get("NextToken")

        break

    return all_blocks


# -----------------------------
# Table parsing + extraction
# -----------------------------


@dataclass(frozen=True)
class _HeaderHit:
    table_id: str
    page: int
    des_row: int
    des_col: int
    qty_col: int
    sales_col: int | None


def _extract_products_and_qty_from_blocks(
    blocks: list[dict[str, Any]],
) -> dict[str, float]:
    block_map: dict[str, dict[str, Any]] = {b["Id"]: b for b in blocks if "Id" in b}

    # Group TABLE blocks by page for more predictable extraction.
    tables: list[dict[str, Any]] = [b for b in blocks if b.get("BlockType") == "TABLE"]

    records: dict[str, float] = {}
    weekly_sales_total: float | None = None

    for table in tables:
        hit = _find_header_and_qty_col(table, block_map)
        if hit is None:
            continue

        table_records = _extract_rows_from_table(
            table=table,
            block_map=block_map,
            des_row=hit.des_row,
            des_col=hit.des_col,
            qty_col=hit.qty_col,
        )

        # If we got something meaningful, keep it.
        if table_records:
            records.update(table_records)
            if weekly_sales_total is None and hit.sales_col is not None:
                weekly_sales_total = _extract_sales_total_from_table(
                    table=table,
                    block_map=block_map,
                    des_row=hit.des_row,
                    sales_col=hit.sales_col,
                )

    # If multiple tables matched (rare), you can dedupe here if needed.
    if weekly_sales_total is not None:
        records[SALES_TOTAL_KEY] = weekly_sales_total
    return records


def _find_header_and_qty_col(
    table: dict[str, Any],
    block_map: dict[str, dict[str, Any]],
) -> _HeaderHit | None:
    page = int(table.get("Page", 1))
    table_id = table["Id"]

    grid, _, max_col = _table_to_grid(table, block_map)

    # Find "Désignation" cell in the grid.
    des_row = des_col = None
    for (r, c), text in grid.items():
        if _is_designation_header(text):
            des_row, des_col = r, c
            break

    if des_row is None or des_col is None:
        return None

    # In the same header row, find the right "Qte" column.
    header_cells: list[tuple[int, str]] = [
        (c, grid.get((des_row, c), "")) for c in range(1, max_col + 1)
    ]

    qte_cols = [c for (c, t) in header_cells if _is_standalone_qte(t)]
    sales_col = next((c for (c, t) in header_cells if _is_vente_header(t)), None)

    if not qte_cols:
        return None

    # Preferred: the "Qte" immediately left of "Poids" in this header row.
    poids_col = None
    for c, t in header_cells:
        if _norm_text(t) == "poids":
            poids_col = c
            break

    qty_col: int | None = None
    if poids_col is not None:
        left_qtes = [c for c in qte_cols if c < poids_col]
        if left_qtes:
            qty_col = max(left_qtes)

    # Fallback: rightmost standalone "Qte" in header row.
    if qty_col is None:
        qty_col = max(qte_cols)

    return _HeaderHit(
        table_id=table_id,
        page=page,
        des_row=des_row,
        des_col=des_col,
        qty_col=qty_col,
        sales_col=sales_col,
    )


def _extract_rows_from_table(
    *,
    table: dict[str, Any],
    block_map: dict[str, dict[str, Any]],
    des_row: int,
    des_col: int,
    qty_col: int,
) -> dict[str, float]:
    grid, max_row, _ = _table_to_grid(table, block_map)

    out: dict[str, float] = {}

    for r in range(des_row + 1, max_row + 1):
        name = _clean_cell_text(grid.get((r, des_col), ""))
        if name == "" or name == "Qte":
            break

        # Skip accidental header repeats inside the body
        if _is_designation_header(name):
            continue

        qty_raw = _clean_cell_text(grid.get((r, qty_col), ""))
        qty = _parse_quantity(qty_raw)

        if qty is not None:
            out[name] = qty

    return out


def _extract_sales_total_from_table(
    *,
    table: dict[str, Any],
    block_map: dict[str, dict[str, Any]],
    des_row: int,
    sales_col: int | None,
) -> float | None:
    if sales_col is None:
        return None

    grid, max_row, _ = _table_to_grid(table, block_map)
    last_num: float | None = None
    for r in range(des_row + 1, max_row + 1):
        val = _clean_cell_text(grid.get((r, sales_col), ""))
        num = _parse_quantity(val)
        if num is not None:
            last_num = num
    return last_num


def _table_to_grid(
    table: dict[str, Any],
    block_map: dict[str, dict[str, Any]],
) -> tuple[dict[tuple[int, int], str], int, int]:
    """
    Build a simple (row, col) -> text mapping for CELL blocks.
    We ignore MERGED_CELL blocks on purpose.
    """
    cell_ids: list[str] = []
    for rel in table.get("Relationships", []):
        if rel.get("Type") == "CHILD":
            cell_ids.extend(rel.get("Ids", []))

    grid: dict[tuple[int, int], str] = {}
    max_row = 0
    max_col = 0

    for cid in cell_ids:
        b = block_map.get(cid)
        if not b:
            continue
        if b.get("BlockType") != "CELL":
            # Ignore MERGED_CELL or anything else.
            continue

        r = int(b.get("RowIndex", 0))
        c = int(b.get("ColumnIndex", 0))
        if r <= 0 or c <= 0:
            continue

        text = _cell_text(b, block_map)
        text = _clean_cell_text(text)

        grid[(r, c)] = text
        max_row = max(max_row, r)
        max_col = max(max_col, c)

    return grid, max_row, max_col


def _cell_text(cell_block: dict[str, Any], block_map: dict[str, dict[str, Any]]) -> str:
    parts: list[str] = []
    for rel in cell_block.get("Relationships", []):
        if rel.get("Type") != "CHILD":
            continue
        for cid in rel.get("Ids", []):
            child = block_map.get(cid)
            if not child:
                continue
            bt = child.get("BlockType")
            if bt == "WORD":
                parts.append(child.get("Text", ""))
            elif bt == "SELECTION_ELEMENT":
                if child.get("SelectionStatus") == "SELECTED":
                    parts.append("X")
    return " ".join([p for p in parts if p])


# -----------------------------
# Text normalization/parsing
# -----------------------------


def _clean_cell_text(s: str) -> str:
    s = (s or "").replace("\u00a0", " ")
    s = re.sub(r"\s+", " ", s).strip()
    return s


def _strip_accents(s: str) -> str:
    s = unicodedata.normalize("NFKD", s)
    return "".join(ch for ch in s if not unicodedata.combining(ch))


def _norm_text(s: str) -> str:
    s = _clean_cell_text(s).lower()
    s = _strip_accents(s)
    return s


def _is_designation_header(text: str) -> bool:
    return _norm_text(text) == "designation"


_QTE_RE = re.compile(r"^qte\.?$", re.IGNORECASE)


def _is_standalone_qte(text: str) -> bool:
    # Must be a standalone header cell, not "Diff. Qte ..." etc.
    return bool(_QTE_RE.match(_strip_accents(_clean_cell_text(text)).lower()))


def _is_vente_header(text: str) -> bool:
    # Matches "Vente $" or "Ventes $" (with/without spaces or $)
    s = _strip_accents(_clean_cell_text(text)).lower()
    s = re.sub(r"[\s$]", "", s)
    return s in {"vente", "ventes"}

def _parse_quantity(v: str) -> float | None:
    """
    Parse quantities like: 14, 0, 1 234, 12,5, etc.
    Returns int if integer-like, else float; None if cannot parse.
    """
    s = _clean_cell_text(v)
    if s == "":
        return None

    s = s.replace(" ", "").replace("\u00a0", "")
    s = s.replace(",", ".")
    m = re.search(r"-?\d+(?:\.\d+)?", s)
    if not m:
        return None

    x = float(m.group(0))
    return x


if __name__ == "__main__":
    path = Path("./data/8187 St-Juile/8187.pdf")
    handle_pdf(path)
