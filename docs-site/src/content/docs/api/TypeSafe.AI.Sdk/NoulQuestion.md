---
title: "NoulQuestion"
description: "A yes/no question with optional descriptions for either outcome."
editUrl: false
prev: false
next: false
sidebar:
  label: "NoulQuestion"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_NoulQuestion"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class NoulQuestion : Question
```


A yes/no question with optional descriptions for either outcome.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Question](/api/TypeSafe.AI.Sdk/Question/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L190)

## Constructors

<a id="TypeSafe_AI_Sdk_NoulQuestion__ctor_"></a>
<a id="TypeSafe_AI_Sdk_NoulQuestion__ctor_System_Object_TypeSafe_AI_Sdk_NoulCriteria_System_Boolean_System_Boolean_"></a>

### NoulQuestion(object?, NoulCriteria?, bool, bool)


```csharp
public NoulQuestion(object? instructions = null, NoulCriteria? criteria = null, bool includeInstructions = true, bool includeCriteria = false)
```


Create a Noul question.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `criteria` | [NoulCriteria](/api/TypeSafe.AI.Sdk/NoulCriteria/) |  |
| `includeInstructions` | [bool](https://learn.microsoft.com/dotnet/api/system.boolean) |  |
| `includeCriteria` | [bool](https://learn.microsoft.com/dotnet/api/system.boolean) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L201)

## Properties

<a id="TypeSafe_AI_Sdk_NoulQuestion_Criteria_"></a>
<a id="TypeSafe_AI_Sdk_NoulQuestion_Criteria"></a>

### Criteria


```csharp
public NoulCriteria? Criteria { get; }
```


Optional descriptions of the yes and no outcomes.

**Value:** [NoulCriteria](/api/TypeSafe.AI.Sdk/NoulCriteria/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L196)

<a id="TypeSafe_AI_Sdk_NoulQuestion_Type_"></a>
<a id="TypeSafe_AI_Sdk_NoulQuestion_Type"></a>

### Type


```csharp
public override string Type { get; }
```


Question type: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L193)
