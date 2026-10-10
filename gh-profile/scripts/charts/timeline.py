"""Contributions per month as one continuous run of columns, oldest year first.

Every month shares one scale, so growth reads as growth. Quiet years would
otherwise vanish into the baseline, so each year's total sits under its label.
"""

import datetime as dt

from .github import YearContributions
from .svg import HEADER, PAD, WIDTH, Svg, column_path, fmt, nice_ticks
from .theme import MONTHS, Theme

NAME = "timeline"
NEEDS = "contributions"

PLOT_H, AXIS_H, KEY_H, LABEL_W, BAR_GAP = 150, 44, 22, 36, 2


def render(years: list[YearContributions], t: Theme, today: dt.date) -> str:
    # One entry per elapsed month: (year, month index, count).
    months = [
        (y.year, m, v)
        for y in years
        for m, v in enumerate(y.months)
        if not (y.year == today.year and m + 1 > today.month)
    ]
    total = sum(v for _, _, v in months)
    last_twelve = sum(v for _, _, v in months[-12:])
    share = round(100 * last_twelve / total) if total else 0
    peak = max(months, key=lambda p: p[2])

    x0, x1 = PAD + LABEL_W, WIDTH - PAD
    y0 = HEADER + 22  # room for the peak label above the tallest column
    y1 = y0 + PLOT_H
    # Scale to the peak rather than the next round tick, which can sit far above it.
    top = peak[2] * 1.08 or 1
    step = nice_ticks(peak[2])[0][1]
    ticks = [step * i for i in range(int(top // step) + 1)]
    slot = (x1 - x0) / len(months)
    bar_w = max(1.5, slot - BAR_GAP)
    ongoing = months[-1][0] == today.year and months[-1][1] + 1 == today.month

    def sy(v: float) -> float:
        return y1 - (y1 - y0) * v / top

    svg = Svg(
        t,
        "Contributions per month",
        f"{fmt(total)} since {years[0].year} · {share}% of them in the last twelve months · updated {today.isoformat()}",
    )
    svg.height = y1 + AXIS_H + (KEY_H if ongoing else 0)
    svg.y_axis(ticks, sy, x0, x1)

    for i, (year, m, v) in enumerate(months):
        if v <= 0:
            continue
        x = x0 + slot * i + (slot - bar_w) / 2
        partial = ongoing and i == len(months) - 1
        svg.path(column_path(x, y1, bar_w, max(1.5, y1 - sy(v)), r=min(2, bar_w / 2)), t.accent_soft if partial else t.accent)
        if (year, m) == peak[:2]:
            label = f"{fmt(v)} in {MONTHS[m]} {year}"
            anchor = "end" if x > (x0 + x1) / 2 else "start"
            svg.text(x + bar_w if anchor == "end" else x, sy(v) - 7, label, fill=t.ink2, anchor=anchor, nums=True)

    # Year bands: a tick at each January, the year and its total centered under it.
    starts = [i for i, (_, m, _) in enumerate(months) if m == 0 or i == 0]
    for k, i in enumerate(starts):
        end = starts[k + 1] if k + 1 < len(starts) else len(months)
        if i > 0:
            svg.parts.append(
                f'<line x1="{x0 + slot * i:.1f}" y1="{y1:.1f}" x2="{x0 + slot * i:.1f}" y2="{y1 + 6:.1f}" '
                f'stroke="{t.grid}" stroke-width="1"/>'
            )
        cx = x0 + slot * (i + end) / 2
        year = months[i][0]
        year_total = sum(v for y, _, v in months if y == year)
        svg.text(cx, y1 + 18, str(year), 12, t.ink2, "middle", "600")
        svg.text(cx, y1 + 33, fmt(year_total), 11, anchor="middle", nums=True)

    if ongoing:
        ky = y1 + AXIS_H + 10
        svg.swatch(x0, ky, t.accent_soft)
        svg.text(x0 + 16, ky, f"{MONTHS[today.month - 1]} {today.year} so far")
    return svg.render()
