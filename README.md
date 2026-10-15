# TypeSafe.AI.Sdk

Community [.NET](https://dotnet.microsoft.com) client for [TypeSafe AI](https://typesafe.ai) — a port of [`@typesafe-ai/sdk`](https://github.com/typesafe-ai/typesafe-sdk-js) by Dario Kondratiuk.

This is **not** an official TypeSafe product and is not endorsed by TypeSafe. Behavior is aligned with the JavaScript SDK and the [TypeSafe docs](https://docs.typesafe.ai/sdk).

[![CI](https://github.com/hardkoded/typesafe-sdk-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/hardkoded/typesafe-sdk-dotnet/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/TypeSafe.AI.Sdk.svg)](https://www.nuget.org/packages/TypeSafe.AI.Sdk)

**Package id:** `TypeSafe.AI.Sdk`  
**Docs site:** https://hardkoded.github.io/typesafe-sdk-dotnet/

## Install

```bash
dotnet add package TypeSafe.AI.Sdk
```

Targets `net10.0` (latest LTS) and `netstandard2.0`. Requires the .NET 10 SDK to build (`global.json`).

## Quickstart

Set `TYPESAFE_API_KEY` in your environment, then:

```csharp
using TypeSafe.AI.Sdk;

using var client = new TypeSafeClient();
var response = await client.SystemOneAsync(
    new { document = "I was charged twice. Please fix this ASAP." },
    ("category", Question.Choice("What is this ticket about?",
        ("billing", null),
        ("technical", null),
        ("other", null))));

Console.WriteLine(response.GetChoice("category").Choice);
```

That mirrors the [JavaScript quickstart](https://docs.typesafe.ai/sdk/javascript.md). You can also pass `apiKey` to the constructor.

## Questions

| Type | Builder | Answer |
| --- | --- | --- |
| Choice | `Question.Choice(instructions, criteria)` | `choice`, `probabilities`, `confidence` |
| Score | `Question.Score(instructions, level0, level1, …)` | `score`, `legend`, `probabilities`, `confidence` |
| Noul | `Question.Noul(instructions, criteria?)` | `noul` (P(yes), 0–1) |

Batch independent questions in one `SystemOneAsync` call. See [primitives](https://docs.typesafe.ai/primitives.md).

## Images

Create an image with `new Image(bytes)` (or `ReadOnlyMemory<byte>` / `Stream`) from PNG or JPEG file bytes, or with `Image.FromBase64()` from base64 text. Images must be PNG or JPEG; the API detects the format from the file bytes and rejects other formats, such as WebP and GIF, with a `BadRequestException`. There are three ways to use an image in a request, and they can be combined.

Image support is experimental. The JavaScript SDK does **not** set `TypeSafe-Danger-Zone-Images` automatically, and this port matches that: opt in yourself via `DefaultHeaders` or per-call `RequestOptions.Headers`:

```csharp
using var client = new TypeSafeClient(new TypeSafeClientOptions
{
    DefaultHeaders = new Dictionary<string, string>
    {
        ["TypeSafe-Danger-Zone-Images"] = "true",
    },
});
```

### Place the image in the content

Put the image anywhere in `state`, question instructions, or criteria:

```csharp
var photo = new Image(await File.ReadAllBytesAsync("returns/RMA-1042.jpg"));

var result = await client.SystemOneAsync(
    new { claim = "The mug arrived with a chipped rim.", photo },
    ("damaged", Question.Noul("Is the product in the photo damaged?")),
    ("severity", Question.Score("How severe is the damage?", "none", "cosmetic", "unusable")));
```

C# has no tagged templates. An image interpolated into a **plain** string (`$"Compare {photo}"`) is **not** attached; the call throws `TypeSafeException` before send. Use `WithAssets.Format` / `WithAssets.Join` instead.

### Write the image into text with `WithAssets`

```csharp
var upload = new Image(await File.ReadAllBytesAsync("upload.jpg"));
var catalog = new Image(await File.ReadAllBytesAsync("catalog.png"));

var result = await client.SystemOneAsync(
    WithAssets.Format($"A seller uploaded {upload} for a listing whose catalog photo is {catalog}."),
    ("sameProduct", Question.Noul(WithAssets.Format($"Does {upload} show the same product as {catalog}?"))),
    ("quality", Question.Choice(WithAssets.Format($"Is {upload} usable as a listing photo?"),
        ("usable", null),
        ("blurry", null),
        ("cropped", null),
        ("notAProduct", null))));
```

Each image is sent once, however many times it is used. To write a list of images, join it with `WithAssets.Join`:

```csharp
var state = WithAssets.Format($"The seller uploaded {WithAssets.Join(uploads, ", ")}.");
```

### Choose your own references with `Assets`

Pass images in `SystemOneRequest.Assets`, keyed by a reference of your choice, to reference them from plain strings:

```csharp
var result = await client.SystemOneAsync(new SystemOneRequest
{
    State = "A customer returned <delivered>. The catalog photo is <catalog>.",
    Questions = Question.Map(
        ("matches", Question.Noul("Is <delivered> the product shown in <catalog>?"))),
    Assets = new Dictionary<string, Image>
    {
        ["<delivered>"] = new Image(await File.ReadAllBytesAsync("returns/RMA-1042.jpg")),
        ["<catalog>"] = new Image(await File.ReadAllBytesAsync("catalog/mug.png")),
    },
});
```

Use distinct delimiters, such as `<name>`, so references don't match ordinary text. Every name in `Assets` must appear in the request; otherwise the call rejects with a `TypeSafeException` before sending.

Requests with images are sent as `multipart/form-data`, with the JSON payload in a `request` part and each image's file bytes in its own `asset-N` part.

## Configuration

Explicit options win, then environment variables, then SDK defaults (same as JS).

| Option | Environment | Default |
| --- | --- | --- |
| `ApiKey` | `TYPESAFE_API_KEY` | required |
| `BaseUrl` | `TYPESAFE_BASE_URL` | `https://api.typesafe.ai` |
| `DefaultModel` | `TYPESAFE_DEFAULT_MODEL` | `jev-latest` |
| `LogLevel` | `TYPESAFE_LOG_LEVEL` | `warn` |
| `TimeoutMs` | — | `10000` per attempt |
| `Retry` | — | 2 retries, 500–5000ms backoff, 25% jitter |

Retryable HTTP statuses: 408, 429, 500–599. Connection errors and timeouts retry by default. `Retry-After` / `retry-after-ms` are honored up to 60s.

```csharp
using var client = new TypeSafeClient(new TypeSafeClientOptions
{
    ApiKey = "tsk_...",
    TimeoutMs = 20_000,
    Retry = new RetryPolicy { MaxRetries = 4 },
});
```

### Dependency injection

```csharp
services.AddTypeSafeClient(options =>
{
    options.ApiKey = builder.Configuration["TYPESAFE_API_KEY"];
});

public sealed class Tickets(ITypeSafeClient typesafe) { /* ... */ }
```

Uses `IHttpClientFactory` and the named client `TypeSafe.AI`.

## Errors

Exceptions mirror the JS hierarchy (`TypeSafeError` → `TypeSafeException`, and so on):

- `TypeSafeException` — validation / configuration
- `ApiException` — non-2xx after retries (`BadRequestException`, `AuthenticationException`, `PermissionDeniedException`, `NotFoundException`, `UnprocessableEntityException`, `RateLimitException`, `InternalServerException`)
- `ApiConnectionException` / `ApiTimeoutException`
- `ApiUserAbortException` — cancelled `CancellationToken`

## Pack

Versioning is [MinVer](https://github.com/adamralph/minver) from git tags prefixed with `v`:

```bash
git tag v0.3.0
dotnet pack src/TypeSafe.AI.Sdk/TypeSafe.AI.Sdk.csproj -c Release -o artifacts
```

Without a tag, packs are `0.3.0-preview.0` (or a later preview derived from height).
## Tests

```bash
dotnet test
```

Unit tests mock HTTP and do not need a key. Live tests in `tests/TypeSafe.AI.Sdk.IntegrationTests` skip unless `TYPESAFE_API_KEY` is set.

## Sample

```bash
export TYPESAFE_API_KEY=tsk_...
dotnet run --project samples/TypeSafe.AI.Sdk.Sample
```

## Documentation

- TypeSafe concepts and HTTP API: https://docs.typesafe.ai/sdk · https://docs.typesafe.ai/api.md
- JavaScript SDK (parity source): https://github.com/typesafe-ai/typesafe-sdk-js
- This port: [docs site](https://hardkoded.github.io/typesafe-sdk-dotnet/) (DocFX) · [CONTRIBUTING.md](CONTRIBUTING.md) · [agent skill](skills/typesafe-dotnet/SKILL.md)

Build the docs site locally with `dotnet tool update -g docfx && docfx docs/docfx.json --serve`.

