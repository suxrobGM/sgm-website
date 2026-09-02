"""Share of code by language in the repositories started each year."""

import datetime as dt
from collections import Counter

from .github import Repo
from .svg import HEADER, PAD, WIDTH, Svg, bar_path, label_ink, text_width
from .theme import Theme

NAME = "languages"
NEEDS = "repos"

ROW_H, BAR_H, LABEL_W, GAP = 28, 18, 44, 2
TOP_N, MAX_YEARS = 6, 8

# Mostly markup, styling, config or generated code, which would otherwise swamp
# the chart with vendored CSS and notebook JSON.
EXCLUDE = {
    "CSS", "SCSS", "Less", "HTML", "Liquid", "MDX", "Markdown", "TeX", "Jupyter Notebook",
    "Makefile", "Dockerfile", "Batchfile", "Shell", "PowerShell", "XSLT", "Mako", "Hack",
    "PLpgSQL", "TSQL", "Lex", "Yacc", "Smarty", "Handlebars", "Pug", "Vim Script", "CMake",
    "Nix", "HCL", "Procfile", "Roff", "Jinja", "Twig", "Blade", "Razor",
}


def shares(repos: list[Repo]) -> tuple[list[str], dict[int, list[tuple[str, float]]], int]:
    """Top languages overall; per year the (language, fraction) segments in that
    order with the tail folded into "Other"; and how many repos counted."""
    by_year: dict[int, Counter] = {}
    overall: Counter = Counter()
    counted = 0
    for r in repos:
        langs = {k: v for k, v in r.languages.items() if k not in EXCLUDE}
        if langs:
            by_year.setdefault(r.created, Counter()).update(langs)
            overall.update(langs)
            counted += 1
    top = [name for name, _ in overall.most_common(TOP_N)]
    rows: dict[int, list[tuple[str, float]]] = {}
    for year in sorted(by_year)[-MAX_YEARS:]:
        counts = by_year[year]
        total = sum(counts.values())
        segs = [(n, counts[n] / total) for n in top if counts.get(n)]
        if other := sum(v for n, v in counts.items() if n not in top):
            segs.append(("Other", other / total))
        rows[year] = segs
    return top, rows, counted


def render(repos: list[Repo], t: Theme, today: dt.date) -> str:
    top, rows, repo_count = shares(repos)
    palette = dict(zip(top, t.categorical))
    palette["Other"] = t.gray

    x0, x1 = PAD + LABEL_W, WIDTH - PAD
    svg = Svg(
        t,
        "Languages by project start year",
        f"Share of code in the {repo_count} repositories I started, by language · markup and config excluded",
    )
    has_other = any(name == "Other" for segs in rows.values() for name, _ in segs)
    legend = [(palette[n], n) for n in top] + ([(t.gray, "Other")] if has_other else [])
    y0 = svg.legend(legend, x0, HEADER + 4, x1) + 4
    svg.height = y0 + ROW_H * len(rows) + 10

    width = x1 - x0
    for i, (year, segs) in enumerate(rows.items()):
        ry = y0 + ROW_H * i
        svg.text(x0 - 12, ry + BAR_H - 4, str(year), 13, t.ink2, "end", "600")
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
