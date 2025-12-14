from __future__ import annotations

import itertools
import json
import re
from collections import defaultdict
from dataclasses import dataclass
from datetime import datetime
from pathlib import Path
from typing import Any

from openpyxl import Workbook
from openpyxl.styles import Alignment, Font, PatternFill
from openpyxl.utils import get_column_letter


@dataclass(frozen=True)
class ReportRef:
    year: int
    dt: datetime  # parsed from filename YYYYMMDD
    path: Path

    @property
    def mmdd(self) -> str:
        return self.dt.strftime("%m/%d")


def compile_json_reports_to_excel(
    *,
    json_root: str | Path = "json",
    output_xlsx: str | Path = "compiled_sales.xlsx",
) -> Path:
    """
    Reads JSON reports from:
      json/<year>/<store_name>/<YYYYMMDD>.json

    Each JSON is a hashmap { product_name: quantity }.

    Creates an Excel workbook:
      - One sheet per store
      - Rows: products
      - Columns: report dates (header shown as MM/DD) ordered by (MM, DD, Year),
        so same MM/DD across different years appear consecutively (oldest year first).
      - Year-based background colors for report-date columns
      - Year total columns (one per year)
      - Final "All Years Total" column
      - Final "TOTAL" row summing each report column and each year total column
        (bottom-right cell = grand total).
    """
    json_root = Path(json_root)
    output_xlsx = Path(output_xlsx)

    store_reports: dict[str, list[ReportRef]] = _index_reports(json_root)

    wb = Workbook()
    # Remove default sheet
    wb.remove(wb.active)

    for store_name in sorted(store_reports.keys(), key=str.casefold):
        reports = store_reports[store_name]
        if not reports:
            continue

        ws = wb.create_sheet(_unique_sheet_name(wb, store_name))

        # Load all reports for this store
        report_data: dict[tuple[int, str], dict[str, Any]] = {}
        products_set = set()

        for rep in reports:
            key = (rep.year, rep.dt.strftime("%Y%m%d"))
            data = _load_json_map(rep.path)
            # normalize keys a bit
            cleaned = {}
            for k, v in data.items():
                name = _clean_product_name(k)
                if name:
                    cleaned[name] = v
            report_data[key] = cleaned
            products_set.update(cleaned.keys())

        products = sorted(products_set, key=str.casefold)

        years = sorted({r.year for r in reports})
        year_to_color = _assign_year_colors(years)

        # Column order:
        # report columns ordered by (month, day, year), to keep same MM/DD consecutive
        ordered_reports = sorted(reports, key=lambda r: (r.dt.month, r.dt.day, r.year))

        # Build a mapping from report to column index
        # col 1 = Product, cols 2..N = reports, then year totals, then All Years Total
        header_row = 1
        first_data_row = header_row + 1
        first_report_col = 2

        # Headers
        ws.cell(row=header_row, column=1, value="Product")

        col = first_report_col
        report_col_meta: list[tuple[int, ReportRef]] = []
        for rep in ordered_reports:
            ws.cell(row=header_row, column=col, value=rep.mmdd)
            report_col_meta.append((col, rep))
            col += 1

        # Year total columns
        year_total_cols: dict[int, int] = {}
        for y in years:
            ws.cell(row=header_row, column=col, value=f"{y} Total")
            year_total_cols[y] = col
            col += 1

        all_years_total_col = col
        ws.cell(row=header_row, column=all_years_total_col, value="All Years Total")

        last_col = all_years_total_col
        last_report_col = (
            first_report_col + len(ordered_reports) - 1
            if ordered_reports
            else first_report_col - 1
        )

        # Style header
        header_font = Font(bold=True)
        header_align = Alignment(horizontal="center", vertical="center", wrap_text=True)
        for c in range(1, last_col + 1):
            cell = ws.cell(row=header_row, column=c)
            cell.font = header_font
            cell.alignment = header_align

        # Apply year colors to report columns (and their headers)
        for c, rep in report_col_meta:
            fill = year_to_color[rep.year]
            ws.cell(row=header_row, column=c).fill = fill

        # Apply matching colors to year total headers (helps readability)
        for y, c in year_total_cols.items():
            ws.cell(row=header_row, column=c).fill = year_to_color[y]

        # Write product rows
        for i, product in enumerate(products, start=0):
            r = first_data_row + i
            ws.cell(row=r, column=1, value=product)

            # report quantities
            for c, rep in report_col_meta:
                key = (rep.year, rep.dt.strftime("%Y%m%d"))
                val = report_data.get(key, {}).get(product, None)
                if val is None:
                    continue  # leave empty if not sold / missing
                ws.cell(row=r, column=c, value=_to_number_or_none(val))

            # year totals as formulas over that year's report columns
            for y in years:
                cols_for_year = [c for c, rep in report_col_meta if rep.year == y]
                if not cols_for_year:
                    continue

                # Columns are interleaved across years, so we must sum only the
                # specific cells for that year, not a contiguous range.
                cell_refs = ",".join(
                    f"{get_column_letter(c)}{r}" for c in cols_for_year
                )
                ws.cell(
                    row=r,
                    column=year_total_cols[y],
                    value=f"=SUM({cell_refs})",
                )
            # all years total = sum across all report columns (not year totals)
            if last_report_col >= first_report_col:
                start = get_column_letter(first_report_col)
                end = get_column_letter(last_report_col)
                ws.cell(
                    row=r,
                    column=all_years_total_col,
                    value=f"=SUM({start}{r}:{end}{r})",
                )

        # Totals row
        total_row = first_data_row + len(products)
        ws.cell(row=total_row, column=1, value="TOTAL").font = Font(bold=True)

        # Sum each report column down the product rows
        for c in range(first_report_col, last_report_col + 1):
            col_letter = get_column_letter(c)
            ws.cell(
                row=total_row,
                column=c,
                value=f"=SUM({col_letter}{first_data_row}:{col_letter}{total_row - 1})",
            ).font = Font(bold=True)

        # Sum each year total column down the product rows
        for y, c in year_total_cols.items():
            col_letter = get_column_letter(c)
            ws.cell(
                row=total_row,
                column=c,
                value=f"=SUM({col_letter}{first_data_row}:{col_letter}{total_row - 1})",
            ).font = Font(bold=True)

        # Grand total (bottom-right): sum of report totals across ALL report columns
        if last_report_col >= first_report_col:
            start = get_column_letter(first_report_col)
            end = get_column_letter(last_report_col)
            ws.cell(
                row=total_row,
                column=all_years_total_col,
                value=f"=SUM({start}{total_row}:{end}{total_row})",
            ).font = Font(bold=True)

        # Freeze panes (keep header + product col visible)
        ws.freeze_panes = ws["B2"]

        # Basic widths
        ws.column_dimensions["A"].width = 44
        for c in range(2, last_col + 1):
            ws.column_dimensions[get_column_letter(c)].width = 12

        # Light styling for readability
        body_align = Alignment(horizontal="right", vertical="center")
        ws.column_dimensions["A"].width = 44
        for r in range(first_data_row, total_row + 1):
            ws.cell(row=r, column=1).alignment = Alignment(
                horizontal="left", vertical="center", wrap_text=True
            )
            for c in range(2, last_col + 1):
                ws.cell(row=r, column=c).alignment = body_align
                # shade report columns by year (optional but requested)
                # apply fill to body cells too, not only header
                if c <= last_report_col:
                    rep_year = next(
                        (rep.year for cc, rep in report_col_meta if cc == c), None
                    )
                    if rep_year is not None:
                        ws.cell(row=r, column=c).fill = year_to_color[rep_year]
                elif c in year_total_cols.values():
                    # keep year totals colored as well
                    y = next(
                        (yy for yy, cc in year_total_cols.items() if cc == c), None
                    )
                    if y is not None:
                        ws.cell(row=r, column=c).fill = year_to_color[y]

        # Make totals row stand out a bit
        total_fill = PatternFill("solid", fgColor="FFF2CC")  # light yellow
        for c in range(1, last_col + 1):
            ws.cell(row=total_row, column=c).fill = total_fill

    output_xlsx.parent.mkdir(parents=True, exist_ok=True)
    wb.save(output_xlsx)
    return output_xlsx


# -----------------------------
# Helpers
# -----------------------------


def _index_reports(json_root: Path) -> dict[str, list[ReportRef]]:
    """
    Returns: store_name -> list of ReportRef
    Expects: json/<year>/<store>/<YYYYMMDD>.json
    """
    store_reports: dict[str, list[ReportRef]] = defaultdict(list)

    for p in json_root.rglob("*.json"):
        # Expect at least .../<year>/<store>/<file>.json
        parts = p.parts
        if len(parts) < 3:
            continue

        year_str = parts[-3]
        store = parts[-2]
        stem = p.stem

        if not year_str.isdigit():
            continue
        year = int(year_str)

        # filename is YYYYMMDD
        if not re.fullmatch(r"\d{8}", stem):
            continue

        try:
            dt = datetime.strptime(stem, "%Y%m%d")
        except ValueError:
            continue

        # Prefer filename year if mismatch; but keep folder year for grouping/colors
        # We'll keep folder year as the "year" identity (matches your structure).
        store_reports[store].append(ReportRef(year=year, dt=dt, path=p))

    return store_reports


def _load_json_map(path: Path) -> dict[str, Any]:
    with path.open("r", encoding="utf-8") as f:
        data = json.load(f)
    if not isinstance(data, dict):
        raise ValueError(f"Expected JSON object (hashmap) in {path}")
    return data


def _clean_product_name(name: Any) -> str:
    s = "" if name is None else str(name)
    s = s.replace("\u00a0", " ")
    s = re.sub(r"\s+", " ", s).strip()
    return s


def _to_number_or_none(v: Any) -> int | float | None:
    if v is None:
        return None
    if isinstance(v, bool):
        return None
    if isinstance(v, (int, float)):
        return v
    s = str(v).strip().replace("\u00a0", " ")
    s = s.replace(" ", "").replace(",", ".")
    m = re.search(r"-?\d+(?:\.\d+)?", s)
    if not m:
        return None
    x = float(m.group(0))
    return int(x) if x.is_integer() else x


def _assign_year_colors(years: list[int]) -> dict[int, PatternFill]:
    # Pastel-ish fills; cycles if many years.
    palette = [
        "FFE2EFDA",  # light green
        "FFDDEBF7",  # light blue
        "FFFFF2CC",  # light yellow
        "FFFCE4D6",  # light orange
        "FFE7E6E6",  # light gray
        "FFE4DFEC",  # light purple
        "FFD9E1F2",  # periwinkle
        "FFF8CBAD",  # peach
    ]
    out: dict[int, PatternFill] = {}
    for i, y in enumerate(sorted(years)):
        out[y] = PatternFill("solid", fgColor=palette[i % len(palette)])
    return out


def _unique_sheet_name(wb: Workbook, desired: str) -> str:
    # Excel constraints: max 31 chars; cannot contain: : \ / ? * [ ]
    name = re.sub(r"[:\\/?*\[\]]", "_", desired).strip()
    if not name:
        name = "Sheet"
    name = name[:31]

    existing = set(wb.sheetnames)
    if name not in existing:
        return name

    base = name[:28]  # leave room for suffix like _01
    i = 1
    while True:
        candidate = f"{base}_{i:02d}"
        if candidate not in existing:
            return candidate
        i += 1
