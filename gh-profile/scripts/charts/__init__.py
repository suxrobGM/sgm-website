"""Chart modules. Each exposes NAME, NEEDS ("contributions" or "repos") and
render(data, theme, today) -> svg string."""

from . import activity, cumulative, languages, mix

CHARTS = {m.NAME: m for m in (activity, cumulative, mix, languages)}
