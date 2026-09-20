# TypeSafe.AI.Sdk

Community [.NET](https://dotnet.microsoft.com) client for [TypeSafe AI](https://typesafe.ai) — a Hardkoded port of [`@typesafe-ai/sdk`](https://github.com/typesafe-ai/typesafe-sdk-js).

This is **not** an official TypeSafe product and is not endorsed by TypeSafe. Behavior is aligned with the JavaScript SDK and the [TypeSafe docs](https://docs.typesafe.ai/sdk).

[![CI](https://github.com/hardkoded/typesafe-sdk-dotnet/actions/workflows/ci.yml/badge.svg)](https://github.com/hardkoded/typesafe-sdk-dotnet/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/TypeSafe.AI.Sdk.svg)](https://www.nuget.org/packages/TypeSafe.AI.Sdk)

**Package id:** `TypeSafe.AI.Sdk`  
**Docs site:** https://hardkoded.github.io/typesafe-sdk-dotnet/

## Install

```bash
dotnet add package TypeSafe.AI.Sdk
```

Targets `net10.0` (latest LTS), `net8.0`, and `netstandard2.0`. Requires the .NET 10 SDK to build (`global.json`).

## Quickstart

Set `TYPESAFE_API_KEY` in your environment, then:

```csharp
using TypeSafe.AI.Sdk;

using var client = new TypeSafeClient();
var response = await client.SystemOneAsync(
    new { document = "I was charged twice. Please fix this ASAP." },
    new Dictionary<string, Question>
    {
        ["category"] = Question.Choice("What is this ticket about?", new Dictionary<string, object?>
        {
            ["billing"] = null,
            ["technical"] = null,
            ["other"] = null,
        }),
    });

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
git tag v0.1.0
dotnet pack src/TypeSafe.AI.Sdk/TypeSafe.AI.Sdk.csproj -c Release -o artifacts
```

Without a tag, packs are `0.1.0-preview.0` (or a later preview derived from height).

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

## License

MIT. Upstream JS SDK is also MIT.
