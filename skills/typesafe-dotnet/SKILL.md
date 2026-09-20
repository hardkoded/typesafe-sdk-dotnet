---
name: typesafe-dotnet
license: MIT
description: >
  Use the community TypeSafe.AI.Sdk .NET client to ask System One questions
  (Choice, Score, Noul), batch them, handle retries and errors, and read
  probabilities/confidence. Use when adding TypeSafe judgments to a C# / .NET
  app, when porting JS/Python TypeSafe code, or when an agent needs the .NET
  API instead of HTTP or the official JS/Python SDKs.
---

# TypeSafe .NET SDK

This skill is for **`TypeSafe.AI.Sdk`** (`dotnet add package TypeSafe.AI.Sdk`), a Hardkoded community port of [`@typesafe-ai/sdk`](https://github.com/typesafe-ai/typesafe-sdk-js). It is **not** an official TypeSafe product.

Live TypeSafe docs remain the source of truth for concepts. This skill covers the .NET client surface.

- Index: https://docs.typesafe.ai/llms.txt
- HTTP API: https://docs.typesafe.ai/api.md
- JS SDK (parity source): https://docs.typesafe.ai/sdk/javascript.md
- Official agent skill (language-agnostic design): https://docs.typesafe.ai/agent-skill.md

## When to use which question

| Need | Primitive | .NET builder | Answer |
| --- | --- | --- | --- |
| One of a closed set | Choice | `Question.Choice(instructions, criteria)` | `GetChoice(id).Choice`, `.Probabilities`, `.Confidence` |
| Degree on ordered levels | Score | `Question.Score(instructions, level0, level1, …)` ≥ 2 levels | `GetScore(id).Score`, `.Legend`, `.Probabilities`, `.Confidence` |
| Yes / no | Noul | `Question.Noul(instructions, criteria?)` | `GetNoul(id).Noul` in `[0, 1]` — **no** `Confidence` |

- Choice criteria is a **map** of label → description (`null` if the label is enough). Not a list.
- Score criteria is an **ordered list** (index 0, 1, …). Not a map. The API accepts 2–10 levels.
- Noul criteria is optional `{ true, false }` descriptions (`NoulCriteria` / `NoulCriteria.Yes` / `NoulCriteria.No`).
- A Noul of `0.5` means yes and no are equally likely, **not** “medium intensity”. Use Score for a spectrum.
- Question IDs are for your code only. Put the full meaning in `instructions`.
- Ask every independent question over the same state in **one** `SystemOneAsync` call (speculative fan-out). A second request is only needed when the next state or option set depends on an earlier answer.

## Construct state

`state` can be a string, `null`, array, or object (`Dictionary<string, object?>`, anonymous type, `JsonNode`). Prefer named JSON fields. Point instructions at nested values with backticked paths: `` `ticket.messages[0].text` ``.

```csharp
using TypeSafe.AI.Sdk;

using var client = new TypeSafeClient(); // reads TYPESAFE_API_KEY
var result = await client.SystemOneAsync(
    new
    {
        ticket = new { subject = "Duplicate charge", body = "Charged twice. Refund ASAP." },
        policy = "Duplicate charges are refundable.",
    },
    new Dictionary<string, Question>
    {
        ["refund"] = Question.Noul("Does `ticket.body` request a refund?"),
        ["team"] = Question.Choice("Which team should handle `ticket`?", new Dictionary<string, object?>
        {
            ["billing"] = "Payments, invoicing, refunds",
            ["technical"] = "Bugs and outages",
            ["other"] = null,
        }),
        ["urgency"] = Question.Score("How urgent is `ticket`?", "can wait", "this week", "today"),
    },
    cancellationToken: cancellationToken);

var refund = result.GetNoul("refund").Noul;
var team = result.GetChoice("team");
var urgency = result.GetScore("urgency");
```

`instructions` and criteria values may be strings, objects, arrays, or `null` (structured questions). See https://docs.typesafe.ai/primitives/advanced.md.

## Client, env, retries

Environment (empty/whitespace ignored; constructor wins):

| Variable | Purpose | Default |
| --- | --- | --- |
| `TYPESAFE_API_KEY` | Bearer token | required |
| `TYPESAFE_BASE_URL` | API root | `https://api.typesafe.ai` |
| `TYPESAFE_DEFAULT_MODEL` | When request omits `model` | `jev-latest` |
| `TYPESAFE_LOG_LEVEL` | `debug` `info` `warn` `error` `off` | `warn` |

Retry defaults match JS: `MaxRetries = 2`, `BackoffInitialMs = 500`, `BackoffMaxMs = 5000`, `BackoffJitter = 0.25`, statuses 408/429/5xx, honor `Retry-After` up to 60s, retry connection errors and timeouts. Timeout is **per attempt** (default 10s), not a total budget.

```csharp
services.AddTypeSafeClient(o => o.TimeoutMs = 20_000);
// ITypeSafeClient from DI; named HttpClient: TypeSafeClient.HttpClientName
```

Every public async method takes `CancellationToken`. Cancel → `ApiUserAbortException` (not retried).

## Errors

| .NET | JS | When |
| --- | --- | --- |
| `TypeSafeException` | `TypeSafeError` | Missing key, bad config, empty questions, score &lt; 2 levels |
| `AuthenticationException` | `AuthenticationError` | 401 |
| `RateLimitException` | `RateLimitError` | 429 (`RetryAfterMs`) |
| `UnprocessableEntityException` | `UnprocessableEntityError` | 422 |
| `ApiTimeoutException` | `APITimeoutError` | attempt timeout |
| `ApiConnectionException` | `APIConnectionError` | transport failure |
| `ApiUserAbortException` | `APIUserAbortError` | caller cancel |

Do not invent request/response fields. Usage is `input_tokens` / `output_tokens` (`Usage.InputTokens` / `OutputTokens`). Models: `client.Models.ListAsync()`.

## Agent habits

1. Read https://docs.typesafe.ai/llms.txt and the relevant primitive page before writing questions.
2. Keep questions and thresholds in one file for human review.
3. Use probabilities when you have a statistical rule; use confidence as a second axis for Choice/Score, not as a substitute for Noul.
4. Validate on the user’s data. Cookbook numbers are examples, not defaults.
5. Keep API keys server-side. Never commit `TYPESAFE_API_KEY`.
