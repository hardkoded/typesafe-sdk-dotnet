---
title: "ModelCard"
description: "Metadata for an available model."
editUrl: false
prev: false
next: false
sidebar:
  label: "ModelCard"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_ModelCard"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class ModelCard
```


Metadata for an available model.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L22)

## Properties

<a id="TypeSafe_AI_Sdk_ModelCard_Description_"></a>
<a id="TypeSafe_AI_Sdk_ModelCard_Description"></a>

### Description


```csharp
[JsonPropertyName("description")]
public string Description { get; init; }
```


Human-readable description.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L29)

<a id="TypeSafe_AI_Sdk_ModelCard_Name_"></a>
<a id="TypeSafe_AI_Sdk_ModelCard_Name"></a>

### Name


```csharp
[JsonPropertyName("name")]
public string Name { get; init; }
```


Model identifier, for example `jev-1.13.0`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L25)

<a id="TypeSafe_AI_Sdk_ModelCard_ReleaseDate_"></a>
<a id="TypeSafe_AI_Sdk_ModelCard_ReleaseDate"></a>

### ReleaseDate


```csharp
[JsonPropertyName("release_date")]
public string ReleaseDate { get; init; }
```


Release date as returned by the API.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L33)
