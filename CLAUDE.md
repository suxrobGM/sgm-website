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

## Architecture

**Entry point flow:** `Program.cs` → `Setup.ConfigureServices()` → `Setup.ConfigurePipeline()`

**Key directories:**

- `src/SGM.WebApp/Components/` - Blazor components (Pages, Layout, Shared)
- `src/SGM.WebApp/Services/` - Business logic services (email sender, captcha verification)
- `src/SGM.WebApp/Options/` - Strongly-typed configuration classes bound from appsettings
- `resume/` - LaTeX resume source files and GitHub profile markdown
- `gh-profile/` - GitHub profile README and assets (terminal SVG, screenshots)

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
