# TypeSafe.AI.Sdk docs

The documentation site is [Astro Starlight](https://starlight.astro.build/). The published site is [https://www.hardkoded.com/typesafe-sdk-dotnet/](https://www.hardkoded.com/typesafe-sdk-dotnet/).

## Preview locally

From this directory:

```bash
npm ci
npm run api
npm run dev
```

Open http://localhost:4321/typesafe-sdk-dotnet/ . The path includes `/typesafe-sdk-dotnet/` because that is the base path on https://www.hardkoded.com/typesafe-sdk-dotnet/.

`npm run dev` and `npm run build` copy `guides/` into the site before Astro starts. Edit the guides in `guides/`. The copies under `src/content/docs/` are generated.

Search is built with the production output:

```bash
npm run build
npm run preview
```

## Regenerate the API reference

The API pages are generated. They are not edited by hand.

```bash
npm run api
```

That needs the .NET 10 SDK. It restores the local DocFX tool (`dotnet-tools.json`, DocFX 2.80.1), runs `docfx metadata` on `src/TypeSafe.AI.Sdk` (`net10.0`, `filterConfig.yml`), and writes Markdown into `src/content/docs/api/`. Commit the generated pages and `src/generated/api-sidebar.json`.

`npm run api -- --skip-metadata` rebuilds the Markdown from `docs-site/.api-yaml` without running DocFX again.

## Vercel

The Vercel project root is this directory. Astro writes the site to `dist/` without the `/typesafe-sdk-dotnet` prefix, and the HTML still links to that prefix. On Vercel, `scripts/mount-base.mjs` copies the build under that prefix, and `middleware.ts` rewrites the same prefix onto the root files (including `index.html` for each page). Those URLs then resolve on the Vercel deployment and behind the hardkoded.com multi-zone.

## GitHub Pages

GitHub Pages is retired. `.github/workflows/docs.yml` builds this site on every pull request and on every push to `main`, and uploads `docs-site/dist` as a CI artifact. It does not deploy. Vercel is the host: its Git integration publishes the site on a push to `main`.

Old DocFX `.html` URLs redirect to the new paths. Member anchors from DocFX (`#TypeSafe_AI_Sdk_TypeSafeClient_...`) are kept on the generated pages. A redirect preserves the hash when JavaScript runs.
