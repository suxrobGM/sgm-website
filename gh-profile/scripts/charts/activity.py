"""Contributions per month: one row per year, twelve columns, one shared scale."""

import datetime as dt

from .github import YearContributions
from .svg import HEADER, PAD, WIDTH, Svg, column_path, fmt
from .theme import MONTHS, Theme

NAME = "activity"
NEEDS = "contributions"

ROW_H, BAR_MAX, AXIS_H, KEY_H = 76, 56, 30, 22
LABEL_W, TOTAL_W = 52, 64


def render(years: list[YearContributions], t: Theme, today: dt.date) -> str:
    x0 = PAD + LABEL_W + 12
    x1 = WIDTH - PAD - TOTAL_W - 8
    slot = (x1 - x0) / 12
    bar_w = min(24, slot - 6)

    peak_year, peak_month = max(((y, m) for y in years for m in range(12)), key=lambda p: p[0].months[p[1]])
    peak = peak_year.months[peak_month] or 1
    ongoing = any(y.year == today.year for y in years)

    svg = Svg(
        t,
        "Contributions per month",
        f"Commits, pull requests, issues and reviews · {years[0].year}–{years[-1].year} · "
        f"{fmt(sum(y.total for y in years))} total · updated {today.isoformat()}",
    )
    svg.height = HEADER + ROW_H * len(years) + AXIS_H + (KEY_H if ongoing else 0)

    for i, y in enumerate(years):
        base = HEADER + ROW_H * i + BAR_MAX + 8
        svg.hline(x0, x1, base)
        svg.text(PAD + LABEL_W, base - 3, str(y.year), 13, t.ink2, "end", "600")
        svg.text(WIDTH - PAD, base - 3, fmt(y.total), 12, t.ink2, "end", nums=True)
        for m, v in enumerate(y.months):
            future = y.year == today.year and m + 1 > today.month
            if v <= 0 or future:
                continue
            partial = y.year == today.year and m + 1 == today.month
            h = max(2, v / peak * BAR_MAX)
            x = x0 + slot * m + (slot - bar_w) / 2
            svg.path(column_path(x, base, bar_w, h), t.accent_soft if partial else t.accent)
            if y is peak_year and m == peak_month:
                svg.text(x + bar_w / 2, base - h - 5, fmt(v), fill=t.ink2, anchor="middle", nums=True)

    axis_y = HEADER + ROW_H * len(years) + 14
    for m, name in enumerate(MONTHS):
        svg.text(x0 + slot * m + slot / 2, axis_y, name, anchor="middle")
    if ongoing:
        svg.swatch(x0, axis_y + 20, t.accent_soft)
        svg.text(x0 + 16, axis_y + 20, "current month, still counting")
    return svg.render()
