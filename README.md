# SGM Website

[![Build and Deploy](https://github.com/suxrobGM/sgm-website/actions/workflows/deploy-ssh.yml/badge.svg)](https://github.com/suxrobGM/sgm-website/actions/workflows/deploy-ssh.yml)
[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-SSR-512BD4?logo=blazor&logoColor=white)](https://learn.microsoft.com/aspnet/core/blazor/)
[![Docker](https://img.shields.io/badge/Docker-GHCR-2496ED?logo=docker&logoColor=white)](https://github.com/suxrobGM/sgm-website/pkgs/container/sgm-website%2Fweb)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Live site](https://img.shields.io/website?url=https%3A%2F%2Fsuxrobgm.net&label=suxrobgm.net)](https://suxrobgm.net)

My personal site, live at [suxrobgm.net](https://suxrobgm.net). It's a Blazor app on .NET 10 with
two themes that render the same content, a research page, and a contact form.

| Route       | What it is                                                                      |
| ----------- | ------------------------------------------------------------------------------- |
| `/`         | Vice City: a 1980s Miami game, with a HUD, radar, pause menu and mission log    |
| `/xp`       | Windows XP: the portfolio as desktop windows                                    |
| `/research` | Papers, summaries and BibTeX                                                    |

## Run it

```bash
dotnet run --project src/SGM.WebApp
```

## Editing content

Jobs, projects, skills, research and the about text live in
[`Data/PortfolioData.cs`](src/SGM.WebApp/Data/PortfolioData.cs); papers for the research page
are in [`Data/ResearchData.cs`](src/SGM.WebApp/Data/ResearchData.cs). Both themes pick up
changes automatically.

## Deploy

Pushing to `prod` builds a Docker image, pushes it to GHCR, and restarts it on the VPS with
docker compose. See [`docs/deployment.md`](docs/deployment.md).

## Repository

```text
src/SGM.WebApp/   the site (Blazor app and Dockerfile)
deploy/           docker-compose.yml, .env.example and the nginx site config
docs/             deployment notes
resume/           LaTeX resumes; build.ps1 copies the PDFs into the site
gh-profile/       source of my GitHub profile README
```
