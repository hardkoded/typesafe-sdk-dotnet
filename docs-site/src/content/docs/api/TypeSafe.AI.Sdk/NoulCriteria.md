---
title: "NoulCriteria"
description: "Optional descriptions of the yes and no outcomes for a Noul question."
editUrl: false
prev: false
next: false
sidebar:
  label: "NoulCriteria"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_NoulCriteria"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class NoulCriteria
```


Optional descriptions of the yes and no outcomes for a Noul question.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L162)

## Constructors

<a id="TypeSafe_AI_Sdk_NoulCriteria__ctor_"></a>
<a id="TypeSafe_AI_Sdk_NoulCriteria__ctor_System_Object_System_Object_System_Boolean_System_Boolean_"></a>

### NoulCriteria(object?, object?, bool, bool)


```csharp
public NoulCriteria(object? @true = null, object? @false = null, bool includeTrue = true, bool includeFalse = true)
```


Create Noul criteria, omitting unspecified sides on the wire.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `true` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `false` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |
| `includeTrue` | [bool](https://learn.microsoft.com/dotnet/api/system.boolean) |  |
| `includeFalse` | [bool](https://learn.microsoft.com/dotnet/api/system.boolean) |  |

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L174)

## Properties

<a id="TypeSafe_AI_Sdk_NoulCriteria_False_"></a>
<a id="TypeSafe_AI_Sdk_NoulCriteria_False"></a>

### False


```csharp
public object? False { get; }
```


Description of the no outcome.

**Value:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L168)

<a id="TypeSafe_AI_Sdk_NoulCriteria_True_"></a>
<a id="TypeSafe_AI_Sdk_NoulCriteria_True"></a>

### True


```csharp
public object? True { get; }
```


Description of the yes outcome.

**Value:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L165)

## Methods

<a id="TypeSafe_AI_Sdk_NoulCriteria_No_"></a>
<a id="TypeSafe_AI_Sdk_NoulCriteria_No_System_Object_"></a>

### No(object?)


```csharp
public static NoulCriteria No(object? meaning)
```


Describe only the no outcome.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `meaning` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |

**Returns:** [NoulCriteria](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L186)

<a id="TypeSafe_AI_Sdk_NoulCriteria_Yes_"></a>
<a id="TypeSafe_AI_Sdk_NoulCriteria_Yes_System_Object_"></a>

### Yes(object?)


```csharp
public static NoulCriteria Yes(object? meaning)
```


Describe only the yes outcome.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `meaning` | [object](https://learn.microsoft.com/dotnet/api/system.object) |  |

**Returns:** [NoulCriteria](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Questions/Questions.cs#L183)
