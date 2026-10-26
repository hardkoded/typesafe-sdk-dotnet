---
title: "Answer"
description: "Base type for a typed System One answer."
editUrl: false
prev: false
next: false
sidebar:
  label: "Answer"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_Answer"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public abstract class Answer
```


Base type for a typed System One answer.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

**Derived:** [BoxAnswer](/api/TypeSafe.AI.Sdk/BoxAnswer/), [ChoiceAnswer](/api/TypeSafe.AI.Sdk/ChoiceAnswer/), [NoulAnswer](/api/TypeSafe.AI.Sdk/NoulAnswer/), [ScoreAnswer](/api/TypeSafe.AI.Sdk/ScoreAnswer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L38)

## Properties

<a id="TypeSafe_AI_Sdk_Answer_Type_"></a>
<a id="TypeSafe_AI_Sdk_Answer_Type"></a>

### Type


```csharp
public abstract string Type { get; }
```


Answer type matching the question: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L41)
