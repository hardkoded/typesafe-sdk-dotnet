# Docs site

DocFX site for this repo.

- **URL:** https://hardkoded.github.io/typesafe-sdk-dotnet/
- **Source:** this folder (`docfx.json`, articles, API metadata)
- **Deploy:** `.github/workflows/pages.yml` builds DocFX and publishes `_site` on push to `main`

## Build locally

Requires the .NET 10 SDK (`global.json`) and the DocFX global tool:

```bash
dotnet tool update -g docfx
docfx docs/docfx.json --serve
```

Open the printed local URL (default `http://localhost:8080`).

Enable Pages once: **Settings → Pages → Source: GitHub Actions**.
