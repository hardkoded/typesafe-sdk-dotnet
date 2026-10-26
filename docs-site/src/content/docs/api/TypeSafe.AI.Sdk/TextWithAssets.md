---
title: "TextWithAssets"
description: "Text with the images interpolated into it, sent together with the text."
editUrl: false
prev: false
next: false
sidebar:
  label: "TextWithAssets"
  hidden: true
---

<a id="TypeSafe_AI_Sdk_TextWithAssets"></a>

Namespace: [TypeSafe.AI.Sdk](/api/TypeSafe.AI.Sdk/)


```csharp
public sealed class TextWithAssets
```


Text with the images interpolated into it, sent together with the text.

**Inheritance:** [object](https://learn.microsoft.com/dotnet/api/system.object)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/TextWithAssets.cs#L11)

## Properties

<a id="TypeSafe_AI_Sdk_TextWithAssets_Assets_"></a>
<a id="TypeSafe_AI_Sdk_TextWithAssets_Assets"></a>

### Assets


```csharp
public IReadOnlyList<Image> Assets { get; }
```


The images interpolated into the text, including those in nested [TextWithAssets](./).

**Value:** [IReadOnlyList&lt;Image&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/TextWithAssets.cs#L23)

<a id="TypeSafe_AI_Sdk_TextWithAssets_Text_"></a>
<a id="TypeSafe_AI_Sdk_TextWithAssets_Text"></a>

### Text


```csharp
public string Text { get; }
```


The text, with each image replaced by its reference.

**Value:** [string](https://learn.microsoft.com/dotnet/api/system.string)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/TextWithAssets.cs#L20)

## Methods

<a id="TypeSafe_AI_Sdk_TextWithAssets_From_"></a>
<a id="TypeSafe_AI_Sdk_TextWithAssets_From_System_FormattableString_"></a>

### From(FormattableString)


```csharp
public static TextWithAssets From(FormattableString text)
```


Write images into text: `TextWithAssets.From($"Is {photo} damaged?")`.
Each image is replaced by its reference; other values are converted to strings as in an interpolated string.
An image in a plain interpolated string is rejected before the request is sent.

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `text` | [FormattableString](https://learn.microsoft.com/dotnet/api/system.formattablestring) |  |

**Returns:** [TextWithAssets](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/TextWithAssets.cs#L30)

<a id="TypeSafe_AI_Sdk_TextWithAssets_Join_"></a>
<a id="TypeSafe_AI_Sdk_TextWithAssets_Join_System_Collections_Generic_IEnumerable_System_Object__System_String_"></a>

### `Join(IEnumerable<object?>, string)`


```csharp
public static TextWithAssets Join(IEnumerable<object?> values, string separator = ",")
```


Join &lt;code class="paramref"&gt;values&lt;/code&gt; with &lt;code class="paramref"&gt;separator&lt;/code&gt;, as `String%7d)`
does, and send the images among them with the text. The result can be passed as content or interpolated into
[From(FormattableString)](#TypeSafe_AI_Sdk_TextWithAssets_From_System_FormattableString_).

**Parameters**

| Name | Type | Description |
| --- | --- | --- |
| `values` | [IEnumerable&lt;object&gt;](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1) |  |
| `separator` | [string](https://learn.microsoft.com/dotnet/api/system.string) |  |

**Returns:** [TextWithAssets](./)

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/TextWithAssets.cs#L40)

<a id="TypeSafe_AI_Sdk_TextWithAssets_ToString_"></a>
<a id="TypeSafe_AI_Sdk_TextWithAssets_ToString"></a>

### ToString()


```csharp
public override string ToString()
```


Returns a string that represents the current object.

**Returns:** [string](https://learn.microsoft.com/dotnet/api/system.string) — A string that represents the current object.

[View source](https://github.com/hardkoded/typesafe-sdk-dotnet/blob/main/src/TypeSafe.AI.Sdk/Assets/TextWithAssets.cs#L47)
