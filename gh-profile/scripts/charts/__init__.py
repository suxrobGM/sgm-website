"""Chart modules. Each exposes NAME, NEEDS ("contributions" or "repos") and
render(data, theme, today) -> svg string."""

from . import languages, timeline

CHARTS = {m.NAME: m for m in (timeline, languages)}
