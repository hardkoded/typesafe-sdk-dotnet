---
title: "BoxCoordinates"
description: "A box's edges as fractions of the image's height (top, bottom) and width (left, right)."
editUrl: false
prev: false
next: false
sidebar:
  label: "BoxCoordinates"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_BoxCoordinates"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class BoxCoordinates
```


A box's edges as fractions of the image's height (top, bottom) and width (left, right).

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L93)

## Properties

<a id="TypeSafe_AI_Sdk_BoxCoordinates_Bottom_"></a>
<a id="TypeSafe_AI_Sdk_BoxCoordinates_Bottom"></a>

### Bottom


```csharp
public double Bottom { get; init; }
```


Bottom edge, as a fraction of image height from the top, from zero to one.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L99)

<a id="TypeSafe_AI_Sdk_BoxCoordinates_Left_"></a>
<a id="TypeSafe_AI_Sdk_BoxCoordinates_Left"></a>

### Left


```csharp
public double Left { get; init; }
```


Left edge, as a fraction of image width from the left, from zero to one.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L102)

<a id="TypeSafe_AI_Sdk_BoxCoordinates_Right_"></a>
<a id="TypeSafe_AI_Sdk_BoxCoordinates_Right"></a>

### Right


```csharp
public double Right { get; init; }
```


Right edge, as a fraction of image width from the left, from zero to one.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L105)

<a id="TypeSafe_AI_Sdk_BoxCoordinates_Top_"></a>
<a id="TypeSafe_AI_Sdk_BoxCoordinates_Top"></a>

### Top


```csharp
public double Top { get; init; }
```


Top edge, as a fraction of image height from the top, from zero to one.

**Value:** [double](https://learn.microsoft.com/dotnet/api/system.double)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L96)
