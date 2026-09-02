# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Personal portfolio website for Sukhrob Ilyosbekov built with ASP.NET Core Blazor Server targeting .NET 10. Deployed to <https://suxrobgm.net> via GitHub Actions SSH deployment. The repo also contains resume files and GitHub profile assets.

## Build & Run Commands

```bash
# Build
dotnet build

# Run locally (development)
dotnet run --project src/SGM.WebApp

# Publish for production (matches CI pipeline)
dotnet publish src/SGM.WebApp/SGM.WebApp.csproj -c Release -r linux-x64 -p:PublishSingleFile=true --output ./publish
```

### Resumes (LaTeX)

`resume/build.ps1` compiles the `.tex` resumes with latexmk and copies each PDF into
`src/SGM.WebApp/wwwroot/`, which is where the site serves them from. Compiling without
that copy is how the site ends up serving a stale PDF.

```powershell
./resume/build.ps1              # all three resumes, then sync to wwwroot
./resume/build.ps1 phd          # partial name match: resume-phd only
./resume/build.ps1 aiml -NoSync # compile without touching wwwroot
```

### GitHub profile activity chart

`gh-profile/scripts/activity_chart.py` renders four charts from the GraphQL API as
`assets/<name>-{light,dark}.svg`: `activity` (per month, one row per year),
`cumulative`, `mix` (by contribution type) and `languages` (by project start year).
Drawing code lives in `scripts/charts/`, one module per chart plus `theme`, `svg`
and `github`. The profile repo reruns it daily. Locally: `$env:GITHUB_TOKEN = gh auth token` then
`python gh-profile/scripts/activity_chart.py --user suxrobGM --out gh-profile/assets`
(`--charts mix,languages` for a subset).
The README uses one-column blockquote cards, not multi-column tables, so it reads on
phones; use `<small>` not `<sub>` for captions that may wrap.

## Architecture

**Entry point flow:** `Program.cs` → `Setup.ConfigureServices()` → `Setup.ConfigurePipeline()`

**Key directories:**

- `src/SGM.WebApp/Components/` - Blazor components (Pages, Layout, Shared)
- `src/SGM.WebApp/Services/` - Business logic services (email sender, captcha verification)
- `src/SGM.WebApp/Options/` - Strongly-typed configuration classes bound from appsettings
- `resume/` - LaTeX resume source files and GitHub profile markdown
- `gh-profile/` - Source of truth for the `suxrobGM/suxrobGM` profile repo, same
  layout (`README.md`, `assets/`, `scripts/`, `.github/workflows/`).
  `sync-profile.yml` copies it there on every push to `master` that touches
  `gh-profile/` (needs the `PROFILE_SYNC_TOKEN` secret).

**Pages (themed portfolio variants, all inherit `HomePageBase`):**

- `/` → `HomeViceCity.razor` - GTA Vice City 1980s retro theme (default)
- `/cli` → `HomeTerminal.razor` - Terminal/Matrix hacker theme with CRT effects and scanlines
- `/xp` → `HomeWindowsXP.razor` - Windows XP desktop UI theme (InteractiveServer render mode)
- `/Error` → `Error.razor` - Standard error page

All theme pages share a `ContactForm` component with reCAPTCHA and a `ThemeSwitcher` component for navigating between themes.

**Services:**

- `IEmailSender` / `EmailSender` - Email delivery via Resend API for contact form submissions
- `ICaptchaService` / `RecaptchaEnterpriseService` - Google reCAPTCHA Enterprise validation with risk scoring (threshold >= 0.5)

Both services are registered as scoped in `Setup.ConfigureServices()`.

**Configuration sections in appsettings.json:**

- `EmailConfig` → `EmailSenderOptions` (SenderMail, SenderName, ApiKey)
- `GoogleRecaptcha` → `GoogleRecaptchaOptions` (SiteKey, ProjectId, KeyPath for service account)
- `Serilog` → structured logging to console

## Deployment

Automated via `.github/workflows/deploy-ssh.yml` on push to `prod`. Deploys as a single-file executable to Linux server running as `sgm-main.service` systemd unit.
