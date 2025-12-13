from __future__ import annotations

from pathlib import Path
from collections.abc import Iterable
import json
import re
from typing import Any

import pandas as pd

PDF_EXTS = {".pdf"}
EXCEL_EXTS = {".xls", ".xlsx", ".xlsm", ".xlsb"}


def process_data_subfolder_files(
    data_dir: str | Path,
    *,
    recursive_within_subfolders: bool = False,
) -> None:
    """
    Find all PDF + Excel files contained in *subfolders* of `data_dir`
    (not directly in `data_dir` itself), and dispatch to dedicated handlers.

    - PDF files -> handle_pdf(path)
    - Excel files -> handle_excel(path)
    """
    data_dir = Path(data_dir).resolve()

    if not data_dir.exists() or not data_dir.is_dir():
        raise ValueError(f"data_dir must be an existing directory: {data_dir}")

    # Only look inside immediate subfolders of data_dir (as requested).
    subdirs = [p for p in data_dir.iterdir() if p.is_dir()]

    def iter_files(subdir: Path) -> Iterable[Path]:
        if recursive_within_subfolders:
            yield from subdir.rglob("*")
        else:
            yield from subdir.glob("*")

    for subdir in subdirs:
        for path in iter_files(subdir):
            if not path.is_file():
                continue

            ext = path.suffix.lower()

            if ext in PDF_EXTS:
                handle_pdf(path)
            elif ext in EXCEL_EXTS:
                handle_excel(path)
            # else: ignore everything else


def handle_pdf(path: Path):
    pass


def handle_excel(excel_path: Path, *, json_root: str | Path = "json") -> None:
    """
    Reads an Excel weekly report and extracts:
      - product name from column "Désignation"
      - quantity sold from column "Qte" under the header group "Sommaire hebdo"

    Writes a JSON file to:
      {json_root}/{excel_parent_folder_name}/{excel_stem}.json
    """
    excel_path = Path(excel_path)
    print(f"parsing {excel_path}")

    def norm(v: int | str | None) -> str:
        if v is None:
            return ""
        if isinstance(v, str):
            return " ".join(v.replace("\u00a0", " ").strip().split()).lower()
        return ""

    def is_empty(v: Any) -> bool:
        return v is None or (isinstance(v, str) and v.strip() == "") or pd.isna(v)

    def parse_quantity(v: Any) -> float | None:
        if is_empty(v):
            return None

        # Numeric types
        if isinstance(v, (int, float)) and not pd.isna(v):
            x = float(v)
            return int(x) if x.is_integer() else x

        # Strings like "1 234", "12,5", etc.
        s = str(v).strip().replace("\u00a0", " ")
        s = s.replace(" ", "")
        s = s.replace(",", ".")
        m = re.search(r"-?\d+(?:\.\d+)?", s)
        if not m:
            return None
        x = float(m.group(0))
        return int(x) if x.is_integer() else x

    df = pd.read_excel(excel_path)

    if df.empty:
        return

    nrows, ncols = df.shape

    # 1) Find header row that contains "Désignation"
    des_row = None
    des_col = None
    for r in range(nrows):
        row_vals = df.iloc[r]
        for c, v in enumerate(row_vals):
            if norm(v) == "désignation":
                des_row, des_col = r, c
                break
        if des_row is not None:
            break

    if des_row is None:
        return

    # 2) In row above, find "Sommaire hebdo" and infer its column span
    header_up = df.iloc[des_row - 1]
    som_col = None
    for c, v in enumerate(header_up):
        if norm(v) == "sommaire hebdo":
            som_col = c
            break
    if som_col is None:
        raise ValueError(
            f'Could not find "Sommaire hebdo" above "Désignation" in {excel_path}.'
        )

    # 3) In the "Désignation" header row, find "Qte" within that span
    header_down = df.iloc[des_row]

    qty_col = None
    for c in range(som_col, ncols):
        if norm(header_down[c]) == "qte":
            qty_col = c
            break

        if norm(header_down[c] == "Promo"):
            raise ValueError(f"Could not find Qte in sheet {excel_path}")

    if qty_col is None:
        raise ValueError(
            f'Could not find "Qte" on the same row as "Désignation" in {excel_path}.'
        )

    # Correctness check: they must be on the same row (by construction they are)
    # 4) Extract rows until "Désignation" cell is empty
    items: list[dict[str, Any]] = []
    for r in range(des_row + 1, nrows):
        name_val = df.iat[r, des_col]
        if is_empty(name_val):
            break

        qty_val = df.iat[r, qty_col]
        items.append(
            {
                "product": str(name_val).strip(),
                "quantity": parse_quantity(qty_val),
            }
        )

    # 5) Write JSON output
    out_dir = Path(json_root) / excel_path.parent.name
    out_dir.mkdir(parents=True, exist_ok=True)
    out_path = out_dir / f"{excel_path.stem}.json"

    with out_path.open("w", encoding="utf-8") as f:
        json.dump(items, f, ensure_ascii=False, indent=2)


def main():
    data_path = Path("./data")
    process_data_subfolder_files(data_path)
    # df = pd.read_excel("./data/291 St-Amable/20250803.xlsx")
    # print(df.iloc[9])


if __name__ == "__main__":
    main()
