---
title: "ChoiceQuestion"
description: "A question that selects between named alternatives."
editUrl: false
prev: false
next: false
sidebar:
  label: "ChoiceQuestion"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ChoiceQuestion"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class ChoiceQuestion : Question
```


A question that selects between named alternatives.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Question](/api/TypeSafe.AI.Sdk/Question/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L214)

## Constructors

<a id="TypeSafe_AI_Sdk_ChoiceQuestion__ctor_"></a>
<a id="TypeSafe_AI_Sdk_ChoiceQuestion__ctor_System_Object_System_Collections_Generic_IReadOnlyDictionary_System_String_System_Object__System_Boolean_"></a>

### `ChoiceQuestion(object?, IReadOnlyDictionary<string, object?>, bool)`


```csharp
public ChoiceQuestion(object? instructions, IReadOnlyDictionary<string, object?> criteria, bool includeInstructions = true)
```


Create a Choice question.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `instructions` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `criteria` | [IReadOnlyDictionary&lt;string, object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2) |  |
| `includeInstructions` | [bool](https://learn.microsoft.com/dotnet/api/system.boolean) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L223)

## Properties

<a id="TypeSafe_AI_Sdk_ChoiceQuestion_Criteria_"></a>
<a id="TypeSafe_AI_Sdk_ChoiceQuestion_Criteria"></a>

### Criteria


```csharp
public IReadOnlyDictionary<string, object?> Criteria { get; }
```


Labels mapped to descriptions, or `null` for undescribed labels.

**Value:** [IReadOnlyDictionary&lt;string, object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L220)

<a id="TypeSafe_AI_Sdk_ChoiceQuestion_Type_"></a>
<a id="TypeSafe_AI_Sdk_ChoiceQuestion_Type"></a>

### Type


```csharp
public override string Type { get; }
```


Question type: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L217)
