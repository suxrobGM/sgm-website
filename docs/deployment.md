# Deployment

Push to `prod` → [`deploy-ssh.yml`](../.github/workflows/deploy-ssh.yml) builds the image, pushes to GHCR, then SSHes into the VPS to `docker compose pull && up -d`.

## Pipeline

- **`build`** — matrix-driven; builds each Dockerfile, caches layers in GHCR (`:buildcache`), pushes `:latest` to `ghcr.io/suxrobgm/sgm-website/<image>`. Concurrency cancels superseded builds per image.
- **`deploy`** — writes `.env` and `keys/sa.json` from secrets, SCPs them with `docker-compose.yml` to `/var/www/sgm-main`, then `docker compose up -d --force-recreate`. `cancel-in-progress: false` so deploys never abort mid-flight. Final step curls `127.0.0.1:8000/` as a health probe.

## Compose

[`deploy/docker-compose.yml`](../deploy/docker-compose.yml) — single `sgm-web` service:

- Image: `ghcr.io/${GITHUB_REPOSITORY}/web:${IMAGE_TAG}` (vars appended to `.env` by the deploy step; defaults work for local runs).
- Mounts `./keys:ro` and `./logs`.
- Binds `127.0.0.1:8000 → 8080`. Nginx on the host proxies `suxrobgm.net` to `127.0.0.1:8000`.

## Secrets

| Secret               | Purpose                                                |
| -------------------- | ------------------------------------------------------ |
| `SSH_HOST`           | VPS hostname or IP                                     |
| `SSH_USER`           | SSH user with docker permissions                       |
| `SSH_KEY`            | SSH private key                                        |
| `DOCKER_ENV`         | Multi-line env file content (only true secrets)        |
| `GOOGLE_CREDENTIALS` | reCAPTCHA Enterprise service-account JSON              |

Non-secret config stays in `appsettings.json`. Secrets override it via ASP.NET Core's `Section__Key` env-var convention (e.g. `EmailConfig__ApiKey=re_xxx`). Template at [`deploy/.env.example`](../deploy/.env.example).

## One-time host setup

1. Install docker + docker compose.

Existing nginx config keeps working unchanged.

## Adding another service

Add a row to the build matrix and a matching service in `docker-compose.yml`:

```yaml
- image: api
  context: .
  file: ./src/SGM.Api/Dockerfile
```

## Ops

```bash
docker compose ps              # status
docker compose logs -f sgm-web # tail logs
docker compose pull && docker compose up -d  # manual update
```
