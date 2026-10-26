---
title: "SystemOneRequest"
description: "State and named questions for CancellationToken)."
editUrl: false
prev: false
next: false
sidebar:
  label: "SystemOneRequest"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_SystemOneRequest"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class SystemOneRequest
```


State and named questions for `CancellationToken)`.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L7)

## Properties

<a id="TypeSafe_AI_Sdk_SystemOneRequest_Assets_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneRequest_Assets"></a>

### Assets


```csharp
public IDictionary<string, Image>? Assets { get; set; }
```


Images keyed by a reference of your choice, to reference them from plain strings such as prompts stored in
configuration. Every name must appear in the request; otherwise the call throws before sending.
An image that is also placed in content or in [TextWithAssets](/api/TypeSafe.AI.Sdk/TextWithAssets/) is written there as its name.
Image support is experimental; see [Image](/api/TypeSafe.AI.Sdk/Image/).

**Value:** [IDictionary&lt;string, Image&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.idictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L24)

<a id="TypeSafe_AI_Sdk_SystemOneRequest_Extra_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneRequest_Extra"></a>

### Extra


```csharp
public IDictionary<string, object?>? Extra { get; set; }
```


Additional properties forwarded on the wire, including `null` values.
Matches the JS SDK's extra-field forwarding.

**Value:** [IDictionary&lt;string, object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.idictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L30)

<a id="TypeSafe_AI_Sdk_SystemOneRequest_Model_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneRequest_Model"></a>

### Model


```csharp
public string? Model { get; set; }
```


Model override; omitted values inherit the client `defaultModel`.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L16)

<a id="TypeSafe_AI_Sdk_SystemOneRequest_Questions_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneRequest_Questions"></a>

### Questions


```csharp
public IDictionary<string, Question> Questions { get; set; }
```


Nonempty questions keyed by the names used to identify their answers.

**Value:** [IDictionary&lt;string, Question&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.idictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L13)

<a id="TypeSafe_AI_Sdk_SystemOneRequest_State_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneRequest_State"></a>

### State


```csharp
public object? State { get; set; }
```


Text, a JSON object or array, an [Image](/api/TypeSafe.AI.Sdk/Image/), or `null` to evaluate.

**Value:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Requests/SystemOneRequest.cs#L10)
