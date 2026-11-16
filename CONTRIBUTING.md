# Contributing

This repo is a community .NET port of [`typesafe-ai/typesafe-sdk-js`](https://github.com/typesafe-ai/typesafe-sdk-js). Prefer JS SDK behavior and [docs.typesafe.ai](https://docs.typesafe.ai/llms.txt) over guesswork.

## Layout

| Path | Role |
| --- | --- |
| `src/TypeSafe.AI.Sdk` | Library (`TypeSafe.AI.Sdk`) |
| `tests/TypeSafe.AI.Sdk.Tests` | Unit tests (mocked HTTP) |
| `tests/TypeSafe.AI.Sdk.IntegrationTests` | Live API; skip without `TYPESAFE_API_KEY` |
| `samples/TypeSafe.AI.Sdk.Sample` | Console Choice + Noul |
| `docs-site/` | Starlight docs site (https://www.hardkoded.com/typesafe-sdk-dotnet/) |
| `skills/typesafe-dotnet/` | Agent skill |
| `.github/workflows/` | CI, NuGet publish, docs build |

## Development

Requires the .NET 10 SDK (`global.json`, currently 10.0.401). The library also targets `netstandard2.0` for NuGet consumers on older runtimes.

```bash
dotnet restore
dotnet build
dotnet test
dotnet pack src/TypeSafe.AI.Sdk/TypeSafe.AI.Sdk.csproj -c Release -o artifacts
```

### Code style

Style is enforced at build time:

- Root `.editorconfig` defines naming, IDE, and CA severities.
- `Directory.Build.props` sets `EnforceCodeStyleInBuild`, `EnableNETAnalyzers`, and `TreatWarningsAsErrors`, so style warnings fail `dotnet build` / CI.
- C# files use the MIT file header from `.editorconfig` (`IDE0073`).
- [CSharpier](https://csharpier.com) owns formatting (whitespace and layout), so `IDE0055` is off. It is a local dotnet tool in `.config/dotnet-tools.json`. Run `dotnet tool restore`, then `dotnet csharpier format .`. CI runs `dotnet csharpier check .`.
- A [Husky.Net](https://alirezanet.github.io/Husky.Net/) `pre-push` hook runs `dotnet csharpier check .` and blocks the push if a file is unformatted. `dotnet build` or `dotnet restore` installs the hook for you (`Directory.Build.targets`). It is skipped in CI and when `HUSKY=0`. To install it by hand, run `dotnet tool restore && dotnet husky install`.
- The bulk CSharpier formatting commit is listed in `.git-blame-ignore-revs`. GitHub skips it in blame. For local `git blame`, run `git config blame.ignoreRevsFile .git-blame-ignore-revs`.
- Tests and samples relax selected rules (see the `tests/**/*.cs` / `samples/**/*.cs` sections in `.editorconfig`).

Do not commit API keys or `.env` files.

## Pull requests

- Keep public API close to the JS client: `TypeSafeClient`, `systemOne` → `SystemOneAsync`, `choice` / `score` / `noul`, retries, env vars, error mapping.
- Use idiomatic .NET names (`Exception` suffix, `Async`, `CancellationToken`) and document the JS mapping.
- Add or update unit tests for HTTP, retries, and errors. Integration tests stay gated on `TYPESAFE_API_KEY`.
- XML-document new public APIs.

## Releases

1. Merge to `main`.
2. Tag `vMAJOR.MINOR.PATCH` (MinVer prefix `v`).
3. Push the tag (or re-run **Publish NuGet** via `workflow_dispatch`). To republish an existing tag without retagging, dispatch from `main` with `pack_ref` set to that tag. `publish.yml` packs and pushes to nuget.org with Trusted Publishing (OIDC). No long-lived API key is needed.

## GitHub Pages

GitHub Pages is retired. `docs.yml` builds the Starlight site in `docs-site/` on every pull request and on every push, and uploads the build as a CI artifact. It does not deploy. Vercel Git integration publishes the site on a push to `main`. The site URL is `https://www.hardkoded.com/typesafe-sdk-dotnet/`.

```bash
cd docs-site
npm ci
npm run api
npm run dev
```

Open http://localhost:4321/typesafe-sdk-dotnet/ . Guides live in `docs-site/guides/`. API reference is generated from XML docs on `src/TypeSafe.AI.Sdk` (`npm run api`). Node `>=22.23.4` is required.
