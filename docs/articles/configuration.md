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

## Retries

Retryable HTTP statuses: 408, 429, 500–599. Connection errors and timeouts retry by default. `Retry-After` / `retry-after-ms` are honored up to 60 seconds. Timeout is **per attempt** — there is no total retry budget.

## Per-call overrides

Pass <xref:TypeSafe.AI.Sdk.RequestOptions> to override timeout, retry, or headers for a single `SystemOneAsync` call.
