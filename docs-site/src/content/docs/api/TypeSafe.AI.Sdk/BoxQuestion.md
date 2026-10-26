---
title: "BoxQuestion"
description: "A question that locates what its instructions describe in the request's image."
editUrl: false
prev: false
next: false
sidebar:
  label: "BoxQuestion"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_BoxQuestion"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class BoxQuestion : Question
```


A question that locates what its instructions describe in the request's image.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Question](/api/TypeSafe.AI.Sdk/Question/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L255)

## Constructors

<a id="TypeSafe_AI_Sdk_BoxQuestion__ctor_"></a>
<a id="TypeSafe_AI_Sdk_BoxQuestion__ctor_System_Object_System_Boolean_"></a>

### BoxQuestion(object?, bool)


```csharp
public BoxQuestion(object? instructions = null, bool includeInstructions = true)
```


Create a Box question.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `includeInstructions` | [bool](https://learn.microsoft.com/dotnet/api/system.boolean) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L261)

## Properties

<a id="TypeSafe_AI_Sdk_BoxQuestion_Type_"></a>
<a id="TypeSafe_AI_Sdk_BoxQuestion_Type"></a>

### Type


```csharp
public override string Type { get; }
```


Question type: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L258)
