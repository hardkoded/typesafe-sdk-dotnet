---
title: "RuntimeInfo"
description: "Runtime description written to the X-TypeSafe-Runtime header."
editUrl: false
prev: false
next: false
sidebar:
  label: "RuntimeInfo"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_RuntimeInfo"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public static class RuntimeInfo
```


Runtime description written to the `X-TypeSafe-Runtime` header.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/RuntimeInfo.cs#L9)

## Properties

<a id="TypeSafe_AI_Sdk_RuntimeInfo_Description_"></a>
<a id="TypeSafe_AI_Sdk_RuntimeInfo_Description"></a>

### Description


```csharp
public static string Description { get; }
```


Cached `dotnet/{version} ({os}; {arch})` identifier.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/RuntimeInfo.cs#L12)
