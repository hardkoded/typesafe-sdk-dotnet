---
title: "NoulAnswer"
description: "A yes/no answer. The value is the probability of yes, from 0 to 1."
editUrl: false
prev: false
next: false
sidebar:
  label: "NoulAnswer"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_NoulAnswer"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class NoulAnswer : Answer
```


A yes/no answer. The value is the probability of yes, from 0 to 1.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Answer](/api/TypeSafe.AI.Sdk/Answer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L45)

## Properties

<a id="TypeSafe_AI_Sdk_NoulAnswer_Noul_"></a>
<a id="TypeSafe_AI_Sdk_NoulAnswer_Noul"></a>

### Noul


```csharp
public double Noul { get; init; }
```


Probability of a yes answer, from zero to one.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L51)

<a id="TypeSafe_AI_Sdk_NoulAnswer_Type_"></a>
<a id="TypeSafe_AI_Sdk_NoulAnswer_Type"></a>

### Type


```csharp
public override string Type { get; }
```


Answer type matching the question: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L48)
