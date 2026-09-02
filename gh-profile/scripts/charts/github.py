"""A thin GraphQL client and the two queries the charts need."""

import datetime as dt
import json
import urllib.request
from dataclasses import dataclass

API = "https://api.github.com/graphql"


class Client:
    def __init__(self, token: str):
        self.token = token

    def query(self, query: str, **variables) -> dict:
        body = json.dumps({"query": query, "variables": variables}).encode()
        req = urllib.request.Request(
            API,
            data=body,
            headers={
                "Authorization": f"bearer {self.token}",
                "Content-Type": "application/json",
                "User-Agent": "activity-chart",
            },
        )
        with urllib.request.urlopen(req, timeout=60) as resp:
            payload = json.load(resp)
        if "errors" in payload:
            raise RuntimeError(payload["errors"])
        return payload["data"]


@dataclass
class YearContributions:
    year: int
    months: list[int]  # calendar total per month, January first
    commits: int
    prs: int
    issues: int
    reviews: int
    private: int  # contributions in repos the token cannot itemize

    @property
    def total(self) -> int:
        return sum(self.months)


@dataclass
class Repo:
    name: str
    created: int  # year
    languages: dict[str, int]  # bytes per language


def created_year(client: Client, user: str) -> int:
    data = client.query("query($user: String!) { user(login: $user) { createdAt } }", user=user)
    return int(data["user"]["createdAt"][:4])


_CONTRIBUTIONS = """
query($user: String!, $from: DateTime!, $to: DateTime!) {
  user(login: $user) {
    contributionsCollection(from: $from, to: $to) {
      totalCommitContributions
      totalPullRequestContributions
      totalIssueContributions
      totalPullRequestReviewContributions
      restrictedContributionsCount
      contributionCalendar { weeks { contributionDays { date contributionCount } } }
    }
  }
}
"""


def contributions(client: Client, user: str, year: int) -> YearContributions:
    data = client.query(
        _CONTRIBUTIONS,
        user=user,
        **{"from": f"{year}-01-01T00:00:00Z", "to": f"{year}-12-31T23:59:59Z"},
    )
    col = data["user"]["contributionsCollection"]
    months = [0] * 12
    for week in col["contributionCalendar"]["weeks"]:
        for day in week["contributionDays"]:
            d = dt.date.fromisoformat(day["date"])
            if d.year == year:
                months[d.month - 1] += day["contributionCount"]
    return YearContributions(
        year=year,
        months=months,
        commits=col["totalCommitContributions"],
        prs=col["totalPullRequestContributions"],
        issues=col["totalIssueContributions"],
        reviews=col["totalPullRequestReviewContributions"],
        private=col["restrictedContributionsCount"],
    )


_REPOSITORIES = """
query($user: String!, $cursor: String) {
  user(login: $user) {
    repositories(first: 100, after: $cursor, ownerAffiliations: [OWNER], isFork: false) {
      pageInfo { hasNextPage endCursor }
      nodes {
        nameWithOwner
        createdAt
        languages(first: 10, orderBy: {field: SIZE, direction: DESC}) { edges { size node { name } } }
      }
    }
  }
}
"""


def repositories(client: Client, user: str) -> list[Repo]:
    """Every non-fork repository the user owns."""
    repos: list[Repo] = []
    cursor = None
    while True:
        conn = client.query(_REPOSITORIES, user=user, cursor=cursor)["user"]["repositories"]
        for node in conn["nodes"]:
            repos.append(
                Repo(
                    name=node["nameWithOwner"],
                    created=int(node["createdAt"][:4]),
                    languages={e["node"]["name"]: e["size"] for e in node["languages"]["edges"]},
                )
            )
        if not conn["pageInfo"]["hasNextPage"]:
            return repos
        cursor = conn["pageInfo"]["endCursor"]
