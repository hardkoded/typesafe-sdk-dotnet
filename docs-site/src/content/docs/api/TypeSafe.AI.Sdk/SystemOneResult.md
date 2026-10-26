---
title: "SystemOneResult"
description: "Answers keyed by question name, with model and usage metadata."
editUrl: false
prev: false
next: false
sidebar:
  label: "SystemOneResult"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_SystemOneResult"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class SystemOneResult
```


Answers keyed by question name, with model and usage metadata.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L122)

## Properties

<a id="TypeSafe_AI_Sdk_SystemOneResult_Answers_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneResult_Answers"></a>

### Answers


```csharp
public IReadOnlyDictionary<string, Answer> Answers { get; init; }
```


Answers keyed by the same names supplied in the request.

**Value:** [IReadOnlyDictionary&lt;string, Answer&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L128)

<a id="TypeSafe_AI_Sdk_SystemOneResult_Model_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneResult_Model"></a>

### Model


```csharp
public string Model { get; init; }
```


The model used to answer the request.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L125)

<a id="TypeSafe_AI_Sdk_SystemOneResult_Usage_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneResult_Usage"></a>

### Usage


```csharp
public Usage Usage { get; init; }
```


Token usage for the request.

**Value:** [Usage](/api/TypeSafe.AI.Sdk/Usage/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L132)

## Methods

<a id="TypeSafe_AI_Sdk_SystemOneResult_GetBox_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneResult_GetBox_System_String_"></a>

### GetBox(string)


```csharp
public BoxAnswer GetBox(string name)
```


Return the named Box answer.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `name` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [BoxAnswer](/api/TypeSafe.AI.Sdk/BoxAnswer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L161)

<a id="TypeSafe_AI_Sdk_SystemOneResult_GetChoice_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneResult_GetChoice_System_String_"></a>

### GetChoice(string)


```csharp
public ChoiceAnswer GetChoice(string name)
```


Return the named Choice answer.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `name` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [ChoiceAnswer](/api/TypeSafe.AI.Sdk/ChoiceAnswer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L155)

<a id="TypeSafe_AI_Sdk_SystemOneResult_GetNoul_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneResult_GetNoul_System_String_"></a>

### GetNoul(string)


```csharp
public NoulAnswer GetNoul(string name)
```


Return the named Noul answer.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `name` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [NoulAnswer](/api/TypeSafe.AI.Sdk/NoulAnswer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L152)

<a id="TypeSafe_AI_Sdk_SystemOneResult_GetScore_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneResult_GetScore_System_String_"></a>

### GetScore(string)


```csharp
public ScoreAnswer GetScore(string name)
```


Return the named Score answer.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `name` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [ScoreAnswer](/api/TypeSafe.AI.Sdk/ScoreAnswer/)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L158)

<a id="TypeSafe_AI_Sdk_SystemOneResult_Get_"></a>
<a id="TypeSafe_AI_Sdk_SystemOneResult_Get__1_System_String_"></a>

### `Get<T>(string)`


```csharp
public T Get<T>(string name) where T : Answer
```


Return the named answer as &lt;code class="typeparamref"&gt;T&lt;/code&gt;.

**Type parameters**

| Name | Description |
| --- | --- |
| `T` |  |

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `name` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** `T`

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Responses/Responses.cs#L135)
