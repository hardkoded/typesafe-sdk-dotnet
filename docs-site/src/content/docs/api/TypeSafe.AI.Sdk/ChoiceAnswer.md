---
title: "ChoiceAnswer"
description: "A selected label and its probabilities."
editUrl: false
prev: false
next: false
sidebar:
  label: "ChoiceAnswer"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ChoiceAnswer"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class ChoiceAnswer : Answer
```


A selected label and its probabilities.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Answer](/api/TypeSafe.AI.Sdk/Answer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L55)

## Properties

<a id="TypeSafe_AI_Sdk_ChoiceAnswer_Choice_"></a>
<a id="TypeSafe_AI_Sdk_ChoiceAnswer_Choice"></a>

### Choice


```csharp
public string Choice { get; init; }
```


The selected label.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L61)

<a id="TypeSafe_AI_Sdk_ChoiceAnswer_Confidence_"></a>
<a id="TypeSafe_AI_Sdk_ChoiceAnswer_Confidence"></a>

### Confidence


```csharp
public double Confidence { get; init; }
```


Reported confidence in the selected label.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L64)

<a id="TypeSafe_AI_Sdk_ChoiceAnswer_Probabilities_"></a>
<a id="TypeSafe_AI_Sdk_ChoiceAnswer_Probabilities"></a>

### Probabilities


```csharp
public IReadOnlyDictionary<string, double> Probabilities { get; init; }
```


Probabilities keyed by label.

**Value:** [IReadOnlyDictionary&lt;string, double&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L67)

<a id="TypeSafe_AI_Sdk_ChoiceAnswer_Type_"></a>
<a id="TypeSafe_AI_Sdk_ChoiceAnswer_Type"></a>

### Type


```csharp
public override string Type { get; }
```


Answer type matching the question: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L58)
