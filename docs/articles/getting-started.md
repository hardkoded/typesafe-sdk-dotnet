# Getting started

Install the package and ask your first System One question.

## Install

```bash
dotnet add package TypeSafe.AI.Sdk
```

Targets `net10.0`, `net8.0`, and `netstandard2.0`. Building the library itself requires the .NET 10 SDK.

## API key

Set `TYPESAFE_API_KEY` in your environment, or pass `apiKey` / `TypeSafeClientOptions.ApiKey` explicitly.

```bash
export TYPESAFE_API_KEY=tsk_...
```

## Quickstart

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

That mirrors the [JavaScript quickstart](https://docs.typesafe.ai/sdk/javascript.md). Batch independent questions in one `SystemOneAsync` call.

## Sample project

```bash
export TYPESAFE_API_KEY=tsk_...
dotnet run --project samples/TypeSafe.AI.Sdk.Sample
```

## See also

- [Questions](questions.md)
- [Configuration](configuration.md)
- <xref:TypeSafe.AI.Sdk.TypeSafeClient>
