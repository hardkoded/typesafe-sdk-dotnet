---
title: TypeSafe.AI.Sdk
description: Community .NET client for TypeSafe AI by Hardkoded. Port of @typesafe-ai/sdk.
---

# TypeSafe.AI.Sdk

Hardkoded community port of [`@typesafe-ai/sdk`](https://github.com/typesafe-ai/typesafe-sdk-js).

TypeSafe’s System One models (including Jev) turn application state into structured Choice, Score, and Noul answers your code can use directly. This package is a .NET port — **not** an official TypeSafe product.

```bash
dotnet add package TypeSafe.AI.Sdk
```

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

## Next steps

- [Getting started](articles/getting-started.md) — install, env vars, first request
- [Questions](articles/questions.md) — Choice, Score, and Noul
- [Configuration](articles/configuration.md) — options, retries, timeouts
- [API reference](xref:TypeSafe.AI.Sdk) — generated from the library

## Links

- [TypeSafe Client SDKs](https://docs.typesafe.ai/sdk) · [JavaScript SDK](https://docs.typesafe.ai/sdk/javascript.md) · [HTTP API](https://docs.typesafe.ai/api.md)
- [Primitives](https://docs.typesafe.ai/primitives.md) · [State](https://docs.typesafe.ai/concepts/state.md) · [Confidence](https://docs.typesafe.ai/confidence.md)
- [GitHub](https://github.com/hardkoded/typesafe-sdk-dotnet) · [NuGet](https://www.nuget.org/packages/TypeSafe.AI.Sdk)
