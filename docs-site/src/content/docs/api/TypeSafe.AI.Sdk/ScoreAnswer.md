---
title: "ScoreAnswer"
description: "An expected score with its rubric and probabilities."
editUrl: false
prev: false
next: false
sidebar:
  label: "ScoreAnswer"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ScoreAnswer"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class ScoreAnswer : Answer
```


An expected score with its rubric and probabilities.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Answer](/api/TypeSafe.AI.Sdk/Answer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L72)

## Properties

<a id="TypeSafe_AI_Sdk_ScoreAnswer_Confidence_"></a>
<a id="TypeSafe_AI_Sdk_ScoreAnswer_Confidence"></a>

### Confidence


```csharp
public double Confidence { get; init; }
```


Reported confidence in the score.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L81)

<a id="TypeSafe_AI_Sdk_ScoreAnswer_Legend_"></a>
<a id="TypeSafe_AI_Sdk_ScoreAnswer_Legend"></a>

### Legend


```csharp
public IReadOnlyDictionary<string, JsonElement> Legend { get; init; }
```


Rubric descriptions keyed by score index (string keys matching the API).

**Value:** [IReadOnlyDictionary&lt;string, JsonElement&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L84)

<a id="TypeSafe_AI_Sdk_ScoreAnswer_Probabilities_"></a>
<a id="TypeSafe_AI_Sdk_ScoreAnswer_Probabilities"></a>

### Probabilities


```csharp
public IReadOnlyDictionary<string, double> Probabilities { get; init; }
```


Probabilities keyed by score index.

**Value:** [IReadOnlyDictionary&lt;string, double&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L88)

<a id="TypeSafe_AI_Sdk_ScoreAnswer_Score_"></a>
<a id="TypeSafe_AI_Sdk_ScoreAnswer_Score"></a>

### Score


```csharp
public double Score { get; init; }
```


Expected score, which may fall between integer rubric levels.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L78)

<a id="TypeSafe_AI_Sdk_ScoreAnswer_Type_"></a>
<a id="TypeSafe_AI_Sdk_ScoreAnswer_Type"></a>

### Type


```csharp
public override string Type { get; }
```


Answer type matching the question: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L75)
