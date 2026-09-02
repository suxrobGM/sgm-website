#!/usr/bin/env python3
"""Render GitHub activity charts as light and dark SVGs for a profile README.

Charts, each written as <out>/<name>-light.svg and <out>/<name>-dark.svg:

    activity    contributions per month, one row per year, shared scale
    cumulative  running total of contributions since the first charted year
    mix         commits / pull requests / issues / reviews stacked per year
    languages   share of code by language in repositories started each year

Data comes from the GitHub GraphQL API; private contributions are counted when
the token can see them. Standard library only. See charts/ for the drawing code.

Usage:
    GITHUB_TOKEN=... python scripts/activity_chart.py --user suxrobGM --out assets
    python scripts/activity_chart.py --user suxrobGM --charts activity,mix
"""

import argparse
import datetime as dt
import os
import sys

from charts import CHARTS
from charts.github import (
    Client,
    YearContributions,
    contributions,
    created_year,
    repositories,
)
from charts.theme import THEMES


def select_years(client: Client, user: str, today: dt.date, max_years: int, min_total: int) -> list[YearContributions]:
    """The most recent years worth charting: quiet early years drop out, the
    current year always stays."""
    first = created_year(client, user)
    years = [contributions(client, user, y) for y in range(first, today.year + 1)]
    kept = [y for y in years if y.total >= min_total or y.year == today.year]
    return kept[-max_years:]


def main() -> int:
    ap = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    ap.add_argument("--user", required=True, help="GitHub login to chart")
    ap.add_argument("--out", default="assets", help="output directory for the SVGs")
    ap.add_argument("--charts", default="all", help="comma-separated subset of: " + ",".join(CHARTS))
    ap.add_argument("--max-years", type=int, default=8, help="most recent years to show (default 8)")
    ap.add_argument("--min-total", type=int, default=100, help="skip years below this many contributions")
    args = ap.parse_args()

    names = list(CHARTS) if args.charts == "all" else [x.strip() for x in args.charts.split(",")]
    if unknown := [x for x in names if x not in CHARTS]:
        print(f"unknown chart(s): {', '.join(unknown)}", file=sys.stderr)
        return 2
    token = os.environ.get("GITHUB_TOKEN") or os.environ.get("GH_TOKEN")
    if not token:
        print("GITHUB_TOKEN (or GH_TOKEN) is required", file=sys.stderr)
        return 2

    client = Client(token)
    today = dt.datetime.now(dt.timezone.utc).date()
    needs = {CHARTS[n].NEEDS for n in names}
    data = {}
    if "contributions" in needs:
        data["contributions"] = select_years(client, args.user, today, args.max_years, args.min_total)
    if "repos" in needs:
        data["repos"] = repositories(client, args.user)

    os.makedirs(args.out, exist_ok=True)
    for name in names:
        chart = CHARTS[name]
        for theme in THEMES:
            path = os.path.join(args.out, f"{name}-{theme.name}.svg")
            with open(path, "w", encoding="utf-8", newline="\n") as f:
                f.write(chart.render(data[chart.NEEDS], theme, today))
            print(f"wrote {path}")
    return 0


if __name__ == "__main__":
    sys.exit(main())
