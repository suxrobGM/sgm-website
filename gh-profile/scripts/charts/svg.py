"""A small SVG builder plus the geometry and number helpers the charts share."""

import math
from collections.abc import Callable

from .theme import FONT, HEADER, PAD, WIDTH, Theme


def fmt(n: int) -> str:
    return f"{n:,}"


def fmt_k(n: float) -> str:
    if n >= 1000:
        v = n / 1000
        return f"{v:.0f}K" if v == int(v) else f"{v:.1f}K"
    return f"{n:.0f}"


def esc(s: str) -> str:
    return s.replace("&", "&amp;").replace("<", "&lt;").replace(">", "&gt;")


def nice_ticks(vmax: float, count: int = 3) -> tuple[list[float], float]:
    """Clean tick values (0, step, 2*step, ...) covering vmax, and the top value."""
    if vmax <= 0:
        return [0, 1], 1
    mag = 10 ** math.floor(math.log10(vmax / count))
    step = next(m * mag for m in (1, 2, 2.5, 5, 10) if m * mag * count >= vmax)
    top = step * math.ceil(vmax / step)
    return [step * i for i in range(round(top / step) + 1)], top


def text_width(s: str, size: float) -> float:
    return len(s) * size * 0.58


def luminance(hex_color: str) -> float:
    channels = (int(hex_color[i : i + 2], 16) / 255 for i in (1, 3, 5))
    r, g, b = (c / 12.92 if c <= 0.03928 else ((c + 0.055) / 1.055) ** 2.4 for c in channels)
    return 0.2126 * r + 0.7152 * g + 0.0722 * b


def label_ink(fill: str) -> str:
    """Text color that clears contrast inside a filled segment."""
    return "#0b0b0b" if luminance(fill) > 0.45 else "#ffffff"


def column_path(x: float, y_base: float, w: float, h: float, r: float = 4) -> str:
    """Column with a rounded top (data end) and a square bottom (baseline)."""
    r = min(r, h, w / 2)
    top = y_base - h
    return (
        f"M{x:.1f},{y_base:.1f} V{top + r:.1f} Q{x:.1f},{top:.1f} {x + r:.1f},{top:.1f} "
        f"H{x + w - r:.1f} Q{x + w:.1f},{top:.1f} {x + w:.1f},{top + r:.1f} V{y_base:.1f} Z"
    )


def bar_path(x: float, y: float, w: float, h: float, r: float = 4) -> str:
    """Horizontal bar with a rounded right end and a square left end."""
    r = min(r, w, h / 2)
    right = x + w
    return (
        f"M{x:.1f},{y:.1f} H{right - r:.1f} Q{right:.1f},{y:.1f} {right:.1f},{y + r:.1f} "
        f"V{y + h - r:.1f} Q{right:.1f},{y + h:.1f} {right - r:.1f},{y + h:.1f} H{x:.1f} Z"
    )


class Svg:
    """Collects fragments under a title band. Set `height` once the layout is
    known (legends wrap), then call `render()`."""

    def __init__(self, theme: Theme, title: str, subtitle: str):
        self.t = theme
        self.title = title
        self.subtitle = subtitle
        self.height = 0.0
        self.parts: list[str] = []
        self.text(PAD, 28, title, 17, theme.ink, weight="600")
        self.text(PAD, 50, subtitle, 12)

    def text(
        self,
        x: float,
        y: float,
        s: str,
        size: float = 11,
        fill: str | None = None,
        anchor: str = "start",
        weight: str | None = None,
        nums: bool = False,
    ) -> None:
        attrs = f'x="{x:.1f}" y="{y:.1f}" font-size="{size}" fill="{fill or self.t.muted}"'
        if anchor != "start":
            attrs += f' text-anchor="{anchor}"'
        if weight:
            attrs += f' font-weight="{weight}"'
        if nums:
            attrs += ' style="font-variant-numeric: tabular-nums"'
        self.parts.append(f"<text {attrs}>{esc(s)}</text>")

    def rect(self, x: float, y: float, w: float, h: float, fill: str, rx: float = 0) -> None:
        self.parts.append(f'<rect x="{x:.1f}" y="{y:.1f}" width="{w:.1f}" height="{h:.1f}" rx="{rx}" fill="{fill}"/>')

    def path(self, d: str, fill: str = "none", extra: str = "") -> None:
        self.parts.append(f'<path d="{d}" fill="{fill}"{extra}/>')

    def circle(self, cx: float, cy: float, r: float, fill: str) -> None:
        self.parts.append(f'<circle cx="{cx:.1f}" cy="{cy:.1f}" r="{r}" fill="{fill}"/>')

    def hline(self, x0: float, x1: float, y: float) -> None:
        self.parts.append(
            f'<line x1="{x0:.1f}" y1="{y:.1f}" x2="{x1:.1f}" y2="{y:.1f}" stroke="{self.t.grid}" stroke-width="1"/>'
        )

    def swatch(self, x: float, y: float, color: str) -> None:
        self.rect(x, y - 9, 10, 10, color, rx=2)

    def y_axis(self, ticks: list[float], sy: Callable[[float], float], x0: float, x1: float) -> None:
        """Hairline gridlines with tick labels to the left of x0."""
        for v in ticks:
            self.hline(x0, x1, sy(v))
            self.text(x0 - 8, sy(v) + 4, fmt_k(v), anchor="end", nums=True)

    def legend(self, items: list[tuple[str, str]], x0: float, y: float, max_x: float) -> float:
        """Swatch + label entries, wrapping at max_x. Returns the y below the legend."""
        x = x0
        for color, label in items:
            w = 16 + text_width(label, 11) + 16
            if x + w > max_x and x > x0:
                x, y = x0, y + 18
            self.swatch(x, y, color)
            self.text(x + 15, y, label, fill=self.t.ink2)
            x += w
        return y + 18

    def render(self) -> str:
        h = round(self.height)
        head = [
            f'<svg xmlns="http://www.w3.org/2000/svg" width="{WIDTH}" height="{h}" viewBox="0 0 {WIDTH} {h}" '
            f'role="img" aria-labelledby="t d">',
            f'<title id="t">{esc(self.title)}</title>',
            f'<desc id="d">{esc(self.subtitle)}</desc>',
            f'<g font-family="{FONT}">',
        ]
        return "\n".join(head + self.parts + ["</g></svg>"]) + "\n"


__all__ = [
    "HEADER",
    "PAD",
    "WIDTH",
    "Svg",
    "bar_path",
    "column_path",
    "fmt",
    "fmt_k",
    "label_ink",
    "nice_ticks",
    "text_width",
]
