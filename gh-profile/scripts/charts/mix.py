"""Contributions per year, stacked by type.

GitHub itemizes commits, pull requests, issues and reviews only for repositories
the token can read; the rest arrives as one "restricted" number, drawn in gray.
"""

import datetime as dt

from .github import YearContributions
from .svg import HEADER, PAD, WIDTH, Svg, column_path, fmt, nice_ticks
from .theme import Theme

NAME = "mix"
NEEDS = "contributions"

PLOT_H, AXIS_H, LABEL_W, GAP = 150, 30, 44, 2


def render(years: list[YearContributions], t: Theme, today: dt.date) -> str:
    series = [
        ("commits", "Commits", t.categorical[0]),
        ("prs", "Pull requests", t.categorical[1]),
        ("issues", "Issues", t.categorical[2]),
        ("reviews", "Reviews", t.categorical[3]),
        ("private", "In private repos", t.gray),
    ]
    series = [s for s in series if any(getattr(y, s[0]) for y in years)]
    totals = {y.year: sum(getattr(y, k) for k, _, _ in series) for y in years}

    x0, x1 = PAD + LABEL_W, WIDTH - PAD
    svg = Svg(t, "What the contributions were", f"Per year, by type · {years[0].year}–{years[-1].year} · updated {today.isoformat()}")
    y0 = svg.legend([(color, label) for _, label, color in series], x0, HEADER + 4, x1) + 6
    y1 = y0 + PLOT_H
    svg.height = y1 + AXIS_H

    ticks, top = nice_ticks(max(totals.values()))

    def sy(v: float) -> float:
        return y1 - (y1 - y0) * v / top

    svg.y_axis(ticks, sy, x0, x1)

    slot = (x1 - x0) / len(years)
    bar_w = min(24, slot - 8)
    for i, y in enumerate(years):
        x = x0 + slot * i + (slot - bar_w) / 2
        stack = [(getattr(y, k), color) for k, _, color in series if getattr(y, k) > 0]
        acc = 0
        for j, (v, color) in enumerate(stack):
            offset = GAP if acc > 0 else 0  # surface gap between touching segments
            base = sy(acc) - offset
            h = (y1 - y0) * v / top - offset
            if h >= 1:
                if j == len(stack) - 1:
                    svg.path(column_path(x, base, bar_w, h), color)
                else:
                    svg.rect(x, base - h, bar_w, h, color)
            acc += v
        svg.text(x + bar_w / 2, y1 + 18, str(y.year), anchor="middle")
        if y is years[-1]:
            svg.text(x + bar_w / 2, sy(totals[y.year]) - 6, fmt(totals[y.year]), fill=t.ink2, anchor="middle", nums=True)
    return svg.render()
