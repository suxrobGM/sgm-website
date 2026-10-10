# CLAUDE.md

## What this is

Sukhrob Ilyosbekov's portfolio, live at <https://suxrobgm.net>. ASP.NET Core Blazor on .NET 10,
rendered as static SSR (no interactive circuit except the admin `/pay/new` page). The repo also
holds the LaTeX resumes (`resume/`) and the source of the GitHub profile README (`gh-profile/`).

## Commands

```bash
dotnet run --project src/SGM.WebApp     # local dev
dotnet build                            # fails if a running instance locks bin/; stop it or build with -o <dir>
```

Local-only settings go in `src/SGM.WebApp/appsettings.Development.json` (gitignored).

## Layout

```text
src/SGM.WebApp/
  Program.cs, Setup.cs        services and middleware pipeline
  Data/                       site content as C# records (edit copy here, not in markup)
    PortfolioData.cs          jobs, skills, research highlights, projects, education, about, contacts
    ResearchData.cs           papers with summaries and BibTeX for /research
  Components/
    Pages/                    HomeViceCity (/), HomeWindowsXP (/xp), Research, NotFound, Error, Payment/*
    ViceCity/                 Vice City sections, HUD, pause menu; ViceCityStations.cs lists the sections
    WindowsXP/XpWindow.razor  XP window chrome
    Shared/                   ContactForm, ThemeSwitcher, GameOver, ProjectCard, TagList
  Services/                   email (Resend), reCAPTCHA Enterprise, FreeKassa, StaticAssetVersion
  Controllers/                FreeKassa webhook (api/freekassa/notify)
  wwwroot/
    css/vice-city/, css/windows-xp/   per-theme stylesheets, pulled in by vice-city.css / windows-xp.css
    js/site.js                classic script: cassette music, XP clock/sound, contact form submit
    js/vice-city.js           ES module: HUD, radar, station banner, pause menu, reveals
    images/vc/                Vice City art (WebP), named by each item's Slug
resume/                       LaTeX resumes and build script
gh-profile/                   mirror of the suxrobGM/suxrobGM profile repo
deploy/, docs/                docker-compose and deployment notes
```

## Pages and themes

- **`/` Vice City** (default). `HomeViceCity.razor` only composes components from `Components/ViceCity/`.
  Each section is a radio "station" in `ViceCityStations.cs`, which drives the pause menu (Esc),
  the radar blips and the station banner. Radar blip positions (`60 + i * 100`) must stay in sync
  between `Hud.razor` and `vice-city.js`.
- **`/xp` Windows XP.** One page built from `XpWindow` blocks; `initXpPage()` in `site.js` starts
  the taskbar clock and the startup sound.
- **`/research`.** Plain academic page for PhD applications, fed by `ResearchData`.
- **404 and errors.** Unknown URLs re-execute to `/not-found` ("Wasted"); exceptions go to
  `/Error` ("Busted"). Both use `Shared/GameOver.razor`.
- **Payments.** `/pay/freekassa`, `/pay/status` and the passphrase-locked `/pay/new` link
  generator proxy FreeKassa checkouts for meat.gg; the webhook relays confirmations back.

To add a theme, add a page that inherits `HomePageBase`, reads `PortfolioData`, and a link in `ThemeSwitcher`.

## Things that are easy to get wrong

- **Contact form** is a static form with an enhanced POST. `site.js` catches the submit in the
  capture phase, fetches a reCAPTCHA token into the hidden field, then resubmits. The server
  verifies the token and HTML-encodes the message. Don't give it an interactive render mode.
- **reCAPTCHA badge** is hidden in `App.razor`; `ContactForm` shows the required notice instead.
- **Caching.** Theme stylesheets load their parts through unversioned `@import` URLs, so
  `Setup.cs` forces revalidation of `.css`, `.js` and `.pdf`. Pages and `App.razor` version their
  top-level assets with `StaticAssetVersion.Url()` (injected as `AssetVersion`; `Assets` would hide
  `ComponentBase.Assets`).
- **Resume PDFs** are served from `wwwroot/`, so always rebuild them with the script below.

## Resumes (LaTeX)

`resume/build.ps1` compiles the `.tex` resumes with latexmk and copies each PDF into
`src/SGM.WebApp/wwwroot/`. Compiling without that copy leaves the site serving a stale PDF.

```powershell
./resume/build.ps1              # all three resumes, then sync to wwwroot
./resume/build.ps1 phd          # partial name match: resume-phd only
./resume/build.ps1 aiml -NoSync # compile without touching wwwroot
```

## Vice City art

Generated with Codex CLI's `imagegen` skill (`codex exec -i <reference image> -` with a prompt on
stdin), using `images/myself-vc.jpg` as the character reference and an existing image as the style
reference. Convert the PNG output to WebP before committing (missions 640px, properties 1000px,
hero about 1536px) and don't commit the PNGs.

## GitHub profile

`gh-profile/` mirrors the `suxrobGM/suxrobGM` repo; `.github/workflows/sync-profile.yml` copies it
there on every push to `master` that touches it (needs the `PROFILE_SYNC_TOKEN` secret).

`gh-profile/scripts/activity_chart.py` renders four charts from the GraphQL API as
`assets/<name>-{light,dark}.svg`: `activity`, `cumulative`, `mix` and `languages`. Drawing code
lives in `scripts/charts/`. The profile repo reruns it daily. Locally:

```powershell
$env:GITHUB_TOKEN = gh auth token
python gh-profile/scripts/activity_chart.py --user suxrobGM --out gh-profile/assets   # --charts mix,languages for a subset
```

The profile README uses one-column blockquote cards so it reads on phones; use `<small>`, not
`<sub>`, for captions that may wrap.

## Configuration

`appsettings.json` holds non-secret config; secrets come from env vars (`Section__Key`):

- `EmailConfig` (SenderMail, SenderName, ApiKey) for Resend
- `GoogleRecaptcha` (SiteKey, ProjectId, KeyPath to the service-account JSON); scores below 0.5 fail
- `FreeKassa` (merchant, secrets, meat.gg callback URL, admin key for `/pay/new`)
- `Serilog` writes compact JSON logs to `logs/`

## Deployment

Push to `prod`. `.github/workflows/deploy-ssh.yml` builds the Docker image, pushes it to GHCR, and
runs `docker compose up -d` on the VPS behind nginx. Details in `docs/deployment.md`.
