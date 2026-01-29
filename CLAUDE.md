# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Personal portfolio website for Sukhrob Ilyosbekov built with ASP.NET Core Blazor Server targeting .NET 10. Deployed to <https://suxrobgm.net> via GitHub Actions SSH deployment.

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

**Services:**

- `IEmailSender` / `EmailSender` - SMTP email delivery for contact form
- `ICaptchaService` / `RecaptchaEnterpriseService` - Google reCAPTCHA Enterprise validation

**Configuration sections in appsettings.json:**

- `EmailConfig` → `EmailSenderOptions`
- `GoogleRecaptcha` → `GoogleRecaptchaOptions` (requires Google service account JSON key file)
- `Serilog` → structured logging configuration

## Deployment

Automated via `.github/workflows/deploy-ssh.yml` on push to `master`. Deploys as a single-file executable to Linux server running as `sgm-main.service` systemd unit.

Production secrets (appsettings.Production.json, Google SA key) are managed separately on the deployment server in `/home/persistence/`.
