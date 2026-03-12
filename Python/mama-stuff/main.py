from __future__ import annotations

from pathlib import Path
from collections.abc import Iterable
import json
import re
from typing import Any

import pandas as pd
import itertools

from handle_pdf import handle_pdf
from compile_json_reports_to_excel import compile_json_reports_to_excel

from concurrent.futures import ThreadPoolExecutor

PDF_EXTS = {".pdf"}
EXCEL_EXTS = {".xls", ".xlsx", ".xlsm", ".xlsb"}
SALES_TOTAL_KEY = "__weekly_sales_total__"


def process_data_subfolder_files(
    data_dir: str | Path,
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
    subdirs = list(
        itertools.chain.from_iterable(
            [_p for _p in p.iterdir() if _p.is_dir()]
            for p in data_dir.iterdir()
            if p.is_dir()
        )
    )

    def iter_files(subdir: Path) -> Iterable[Path]:
        yield from subdir.glob("*")

    allFiles = list(
        itertools.chain.from_iterable([[f for f in iter_files(s)] for s in subdirs])
    )

    def fn(path: Path):
        if not path.is_file() or "DS_Store" in path.name:
            return

        ext = path.suffix.lower()
        out_dir = Path(str(path).replace(str(data_dir), "json")).parent
        out_dir.mkdir(parents=True, exist_ok=True)
        out_path = out_dir / f"{path.stem}.json"
        d = None

        if out_path.exists():
            print(f"{path.stem}{path.suffix} exists")
            return

        print(f"parsing {path.stem}{path.suffix}")
        if ext in PDF_EXTS:
            d = handle_pdf(path)
        elif ext in EXCEL_EXTS:
            d = handle_excel(path)

        if d is None:
            return

        with open(out_path, "w") as file:
            json.dump(d, file, indent=2)

    with ThreadPoolExecutor(max_workers=10) as e:
        res = e.map(fn, allFiles)
        res = [r for r in res if r is not None]


def handle_excel(excel_path: Path) -> dict[str, float] | None:
    """
    Reads an Excel weekly report and extracts:
      - product name from column "Désignation"
      - quantity sold from column "Qte" under the header group "Sommaire hebdo"
      - weekly total sales from column "Vente $" under "Sommaire hebdo"

    Writes a JSON file to:
      {json_root}/{excel_parent_folder_name}/{excel_stem}.json
    """
    excel_path = Path(excel_path)

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

    def is_vente_header(v: Any) -> bool:
        s = norm(v)
        s = re.sub(r"[\s$]", "", s)
        return s in {"vente", "ventes"}

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
    vente_col = None
    for c in range(som_col, ncols):
        cell_val = header_down.iloc[c]
        if norm(cell_val) == "qte":
            qty_col = c
            break

        if norm(cell_val) == "promo":
            raise ValueError(f"Could not find Qte in sheet {excel_path}")

    if qty_col is None:
        raise ValueError(
            f'Could not find "Qte" on the same row as "Désignation" in {excel_path}.'
        )

    # Find "Vente $" column in the same header row (if present).
    for c in range(som_col, ncols):
        cell_val = header_down.iloc[c]
        if is_vente_header(cell_val):
            vente_col = c
            break

    # Correctness check: they must be on the same row (by construction they are)
    # 4) Extract rows until "Désignation" cell is empty
    items: dict[str, float] = {}
    for r in range(des_row + 1, nrows):
        name_val = df.iat[r, des_col]
        if is_empty(name_val):
            break

        qty_val = df.iat[r, qty_col]
        q = parse_quantity(qty_val)
        if q is None:
            continue
        items[str(name_val).strip()] = q

    # Weekly sales total: last numeric value in "Vente $" column
    if vente_col is not None:
        last_num: float | None = None
        for r in range(des_row + 1, nrows):
            val = df.iat[r, vente_col]
            num = parse_quantity(val)
            if num is not None:
                last_num = num
        if last_num is not None:
            items[SALES_TOTAL_KEY] = last_num

    return items


def main():
    data_path = Path("./data")
    process_data_subfolder_files(data_path)
    _ = compile_json_reports_to_excel()


if __name__ == "__main__":
    main()
    # d = handle_excel(Path("./data/2026/439 Châteauguy/20260215.xls"))
    # print(d)
