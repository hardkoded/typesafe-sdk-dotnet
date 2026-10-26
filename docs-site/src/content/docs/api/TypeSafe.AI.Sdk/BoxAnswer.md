---
title: "BoxAnswer"
description: "The location of what a box question describes in the image."
editUrl: false
prev: false
next: false
sidebar:
  label: "BoxAnswer"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_BoxAnswer"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class BoxAnswer : Answer
```


The location of what a box question describes in the image.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object) → [Answer](/api/TypeSafe.AI.Sdk/Answer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L109)

## Properties

<a id="TypeSafe_AI_Sdk_BoxAnswer_Box_"></a>
<a id="TypeSafe_AI_Sdk_BoxAnswer_Box"></a>

### Box


```csharp
public BoxCoordinates Box { get; init; }
```


Where the instructions' subject is in the image.

**Value:** [BoxCoordinates](/api/TypeSafe.AI.Sdk/BoxCoordinates/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L115)

<a id="TypeSafe_AI_Sdk_BoxAnswer_Confidence_"></a>
<a id="TypeSafe_AI_Sdk_BoxAnswer_Confidence"></a>

### Confidence


```csharp
public double? Confidence { get; init; }
```


Confidence in the box, from zero to one, or `null` when the API did not report it.

**Value:** [double?](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L118)

<a id="TypeSafe_AI_Sdk_BoxAnswer_Type_"></a>
<a id="TypeSafe_AI_Sdk_BoxAnswer_Type"></a>

### Type


```csharp
public override string Type { get; }
```


Answer type matching the question: `noul`, `choice`, `score`, or `box`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L112)
