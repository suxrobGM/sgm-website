"""Languages in the repositories started each year.

Each repository counts once, split by its share of code, so one large vendored
project can't paint a whole year.
"""

import datetime as dt
from collections import Counter

from .github import Repo
from .svg import HEADER, PAD, WIDTH, Svg, bar_path, label_ink, text_width
from .theme import Theme

NAME = "languages"
NEEDS = "repos"

ROW_H, BAR_H, LABEL_W, COUNT_W, GAP = 28, 18, 44, 64, 2
TOP_N, MAX_YEARS = 7, 8

# Mostly markup, styling, config or generated code, which would otherwise swamp
# the chart with vendored CSS and notebook JSON.
EXCLUDE = {
    "CSS", "SCSS", "Less", "HTML", "Liquid", "MDX", "Markdown", "TeX", "Jupyter Notebook",
    "Makefile", "Dockerfile", "Batchfile", "Shell", "PowerShell", "XSLT", "Mako", "Hack",
    "PLpgSQL", "TSQL", "Lex", "Yacc", "Smarty", "Handlebars", "Pug", "Vim Script", "CMake",
    "Nix", "HCL", "Procfile", "Roff", "Jinja", "Twig", "Blade", "Razor",
}


def shares(repos: list[Repo]) -> tuple[list[str], dict[int, list[tuple[str, float]]], dict[int, int]]:
    """Top languages overall; per year the (language, fraction) segments in that
    order with the tail folded into "Other"; and how many repos each year counted."""
    by_year: dict[int, Counter] = {}
    counts: Counter = Counter()
    overall: Counter = Counter()
    for r in repos:
        langs = {k: v for k, v in r.languages.items() if k not in EXCLUDE}
        if not (size := sum(langs.values())):
            continue
        split = {k: v / size for k, v in langs.items()}
        by_year.setdefault(r.created, Counter()).update(split)
        overall.update(split)
        counts[r.created] += 1
    top = [name for name, _ in overall.most_common(TOP_N)]
    rows: dict[int, list[tuple[str, float]]] = {}
    for year in sorted(by_year)[-MAX_YEARS:]:
        n = counts[year]
        segs = [(lang, by_year[year][lang] / n) for lang in top if by_year[year].get(lang)]
        if other := sum(v for lang, v in by_year[year].items() if lang not in top):
            segs.append(("Other", other / n))
        rows[year] = segs
    return top, rows, {year: counts[year] for year in rows}


def render(repos: list[Repo], t: Theme, today: dt.date) -> str:
    top, rows, counts = shares(repos)
    palette = dict(zip(top, t.categorical))
    palette["Other"] = t.gray

    x0, x1 = PAD + LABEL_W, WIDTH - PAD - COUNT_W
    svg = Svg(
        t,
        "Languages by project start year",
        f"{sum(counts.values())} repositories I started, each counted once and split by its code · markup and config excluded",
    )
    has_other = any(name == "Other" for segs in rows.values() for name, _ in segs)
    legend = [(palette[n], n) for n in top] + ([(t.gray, "Other")] if has_other else [])
    y0 = svg.legend(legend, x0, HEADER + 4, WIDTH - PAD) + 4
    svg.height = y0 + ROW_H * len(rows) + 10

    width = x1 - x0
    for i, (year, segs) in enumerate(rows.items()):
        ry = y0 + ROW_H * i
        svg.text(x0 - 12, ry + BAR_H - 4, str(year), 13, t.ink2, "end", "600")
        n = counts[year]
        svg.text(WIDTH - PAD, ry + BAR_H - 4, f"{n} repo{'s' if n != 1 else ''}", anchor="end", nums=True)
        x = x0
        for j, (name, frac) in enumerate(segs):
            last = j == len(segs) - 1
            w = width * frac - (0 if last else GAP)
            if w >= 1:
                color = palette[name]
                if last:
                    svg.path(bar_path(x, ry, w, BAR_H), color)
                else:
                    svg.rect(x, ry, w, BAR_H, color)
                label = f"{name} {round(100 * frac)}%"
                if text_width(label, 10) + 12 <= w:
                    svg.text(x + w / 2, ry + BAR_H - 5, label, 10, label_ink(color), "middle")
            x += width * frac
    return svg.render()
