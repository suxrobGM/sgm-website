# SGM Website

[![Build and Deploy](https://github.com/suxrobGM/sgm-website/actions/workflows/deploy-ssh.yml/badge.svg)](https://github.com/suxrobGM/sgm-website/actions/workflows/deploy-ssh.yml)
[![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor Server](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor&logoColor=white)](https://learn.microsoft.com/aspnet/core/blazor/)
[![Docker](https://img.shields.io/badge/Docker-GHCR-2496ED?logo=docker&logoColor=white)](https://github.com/suxrobGM/sgm-website/pkgs/container/sgm-website%2Fweb)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Live site](https://img.shields.io/website?url=https%3A%2F%2Fsuxrobgm.net&label=suxrobgm.net)](https://suxrobgm.net)

Sukhrob Ilyosbekov's personal portfolio — live at [suxrobgm.net](https://suxrobgm.net). ASP.NET Core Blazor Server on .NET 10 with three themed homepage variants and a Resend-backed contact form.

## Themes

| Route  | Theme                              |
| ------ | ---------------------------------- |
| `/`    | GTA Vice City 1980s retro          |
| `/cli` | Terminal / Matrix with CRT effects |
| `/xp`  | Windows XP desktop (interactive)   |

## Develop

```bash
dotnet run --project src/SGM.WebApp
```

Local-only settings go in `src/SGM.WebApp/appsettings.Development.json` (gitignored).

## Deploy

Push to `prod` → image built and pushed to GHCR → pulled onto the VPS via docker compose. See [`docs/deployment.md`](docs/deployment.md).

## Layout

```text
src/SGM.WebApp/     Blazor app + Dockerfile
deploy/             docker-compose.yml, .env.example
docs/               Documentation
resume/             LaTeX resume sources
gh-profile/         GitHub profile assets
```
