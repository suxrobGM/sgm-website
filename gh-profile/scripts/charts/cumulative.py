"""Running total of contributions, month by month, since the first charted year."""

import datetime as dt

from .github import YearContributions
from .svg import HEADER, PAD, WIDTH, Svg, fmt, nice_ticks
from .theme import Theme

NAME = "cumulative"
NEEDS = "contributions"

PLOT_H, AXIS_H, LABEL_W, END_W = 130, 30, 44, 64


def render(years: list[YearContributions], t: Theme, today: dt.date) -> str:
    # One point per elapsed month: (year, month index, running total).
    points: list[tuple[int, int, int]] = []
    running = 0
    for y in years:
        for m, v in enumerate(y.months):
            if y.year == today.year and m + 1 > today.month:
                break
            running += v
            points.append((y.year, m, running))
    total = running
    n = len(points)

    x0, x1 = PAD + LABEL_W, WIDTH - PAD - END_W
    y0, y1 = HEADER + 6, HEADER + 6 + PLOT_H
    ticks, top = nice_ticks(total)

    def sx(i: int) -> float:
        return x0 + (x1 - x0) * i / max(1, n - 1)

    def sy(v: float) -> float:
        return y1 - (y1 - y0) * v / top

    last_year = total - (points[-13][2] if n > 13 else 0)
    share = round(100 * last_year / total) if total else 0
    svg = Svg(
        t,
        "Contributions, running total",
        f"Since {years[0].year} · {fmt(total)} so far · {share}% of it in the last twelve months",
    )
    svg.height = y1 + AXIS_H
    svg.y_axis(ticks, sy, x0, x1)

    line = " ".join(f"{'M' if i == 0 else 'L'}{sx(i):.1f},{sy(p[2]):.1f}" for i, p in enumerate(points))
    svg.path(f"{line} L{sx(n - 1):.1f},{y1:.1f} L{sx(0):.1f},{y1:.1f} Z", t.accent, ' fill-opacity="0.1"')
    svg.path(line, extra=f' stroke="{t.accent}" stroke-width="2" stroke-linejoin="round" stroke-linecap="round"')

    ex, ey = sx(n - 1), sy(total)
    svg.circle(ex, ey, 6, t.surface)  # ring so the marker stays legible over the line
    svg.circle(ex, ey, 4, t.accent)
    svg.text(ex + 10, ey + 4, fmt(total), 12, t.ink, weight="600", nums=True)

    for i, (year, m, _) in enumerate(points):
        if m == 0:
            svg.text(sx(i), y1 + 18, str(year), anchor="middle")
    return svg.render()
