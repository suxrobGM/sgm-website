"""Colors and canvas constants shared by every chart.

The SVGs sit on GitHub's own page background, so ink follows GitHub's tokens.
Series colors come from a palette validated for color-vision deficiency and
contrast on both backgrounds: categorical slots in a fixed order plus a neutral
gray for "Other" / unclassified buckets.
"""

from dataclasses import dataclass


@dataclass(frozen=True)
class Theme:
    name: str
    surface: str
    accent: str  # single-series bars and lines
    accent_soft: str  # the month still in progress
    ink: str
    ink2: str
    muted: str
    grid: str
    gray: str
    categorical: tuple[str, ...]


THEMES = (
    Theme(
        name="light",
        surface="#ffffff",
        accent="#2a78d6",
        accent_soft="#86b6ef",
        ink="#1f2328",
        ink2="#59636e",
        muted="#6e7781",
        grid="#d0d7de",
        gray="#8c959f",
        categorical=("#2a78d6", "#eb6834", "#1baf7a", "#eda100", "#e87ba4", "#008300", "#4a3aa7"),
    ),
    Theme(
        name="dark",
        surface="#0d1117",
        accent="#3987e5",
        accent_soft="#1c5cab",
        ink="#e6edf3",
        ink2="#8b949e",
        muted="#8b949e",
        grid="#30363d",
        gray="#6e7681",
        categorical=("#3987e5", "#d95926", "#199e70", "#c98500", "#d55181", "#008300", "#9085e9"),
    ),
)

FONT = "system-ui, -apple-system, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif"
WIDTH = 680  # narrow on purpose so bars stay chunky when GitHub scales to a phone
PAD = 16
HEADER = 66  # title + subtitle band
MONTHS = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"]
