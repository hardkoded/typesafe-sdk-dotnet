# Configuration

Explicit options win, then environment variables, then SDK defaults — same precedence as the JavaScript SDK.

| Option | Environment | Default |
| --- | --- | --- |
| `ApiKey` | `TYPESAFE_API_KEY` | required |
| `BaseUrl` | `TYPESAFE_BASE_URL` | `https://api.typesafe.ai` |
| `DefaultModel` | `TYPESAFE_DEFAULT_MODEL` | `jev-latest` |
| `LogLevel` | `TYPESAFE_LOG_LEVEL` | `warn` |
| `TimeoutMs` | — | `10000` per attempt |
| `Retry` | — | 2 retries, 500–5000ms backoff, 25% jitter |
| `Compression` | — | `Auto` (zstd when available; bodies under 1 KiB stay uncompressed) |

## Client options

```csharp
using var client = new TypeSafeClient(new TypeSafeClientOptions
{
    ApiKey = "tsk_...",
    TimeoutMs = 20_000,
    Retry = new RetryPolicy { MaxRetries = 4 },
});
```

See <xref:TypeSafe.AI.Sdk.TypeSafeClientOptions> and <xref:TypeSafe.AI.Sdk.RetryPolicy>.

## Request compression

Bodies of 1 KiB or more (UTF-8 byte count of the JSON) are compressed with zstd and sent with
`Content-Encoding: zstd`. Smaller bodies are always uncompressed. Compression is applied once per
request and reused on retries; debug logs still show the uncompressed payload. User-supplied
`Content-Encoding` (and `Content-Type` / `Authorization`) cannot clobber the values the SDK sends.

zstd is provided by ZstdSharp.Port on both `net10.0` and `netstandard2.0`. The BCL has no zstd APIs
until .NET 11. <xref:TypeSafe.AI.Sdk.RequestCompression.Auto> (the default) uses zstd when it is
available and otherwise sends uncompressed bodies. <xref:TypeSafe.AI.Sdk.RequestCompression.None>
disables compression. Explicit <xref:TypeSafe.AI.Sdk.RequestCompression.Zstd> throws
<xref:TypeSafe.AI.Sdk.TypeSafeException> if zstd is unavailable.

```csharp
using var client = new TypeSafeClient(new TypeSafeClientOptions
{
    Compression = RequestCompression.None,
});
```

## Retries

Retryable HTTP statuses: 408, 429, 500–599. Connection errors and timeouts retry by default. `Retry-After` / `retry-after-ms` are honored up to 60 seconds. Timeout is **per attempt** — there is no total retry budget.

## Per-call overrides

Pass <xref:TypeSafe.AI.Sdk.RequestOptions> to override timeout, retry, or headers for a single `SystemOneAsync` call.
