---
title: "ScoreQuestion"
description: "A question that assigns a score using an ordered rubric."
editUrl: false
prev: false
next: false
sidebar:
  label: "ScoreQuestion"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ScoreQuestion"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class ScoreQuestion : Question
```


A question that assigns a score using an ordered rubric.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Question](/api/TypeSafe.AI.Sdk/Question/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L234)

## Constructors

<a id="TypeSafe_AI_Sdk_ScoreQuestion__ctor_"></a>
<a id="TypeSafe_AI_Sdk_ScoreQuestion__ctor_System_Object_System_Collections_Generic_IReadOnlyList_System_Object__System_Boolean_"></a>

### `ScoreQuestion(object?, IReadOnlyList<object?>, bool)`


```csharp
public ScoreQuestion(object? instructions, IReadOnlyList<object?> criteria, bool includeInstructions = true)
```


Create a Score question.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `criteria` | [IReadOnlyList&lt;object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1) |  |
| `includeInstructions` | [bool](https://learn.microsoft.com/dotnet/api/system.boolean) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L243)

## Properties

<a id="TypeSafe_AI_Sdk_ScoreQuestion_Criteria_"></a>
<a id="TypeSafe_AI_Sdk_ScoreQuestion_Criteria"></a>

### Criteria


```csharp
public IReadOnlyList<object?> Criteria { get; }
```


Descriptions of the available outcomes, indexed from zero.

**Value:** [IReadOnlyList&lt;object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L240)

<a id="TypeSafe_AI_Sdk_ScoreQuestion_Type_"></a>
<a id="TypeSafe_AI_Sdk_ScoreQuestion_Type"></a>

### Type


```csharp
public override string Type { get; }
```


Question type: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L237)
