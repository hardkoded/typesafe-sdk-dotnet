# Images

Experimental PNG/JPEG support, ported from [`@typesafe-ai/sdk` v0.7.0](https://github.com/typesafe-ai/typesafe-sdk-js). See also [TypeSafe Images](https://docs.typesafe.ai/images) and the [JavaScript SDK](https://docs.typesafe.ai/sdk/javascript.md).

## Opt in

The JavaScript SDK does **not** automatically send `TypeSafe-Danger-Zone-Images`. This port matches that: set the header yourself.

```csharp
using var client = new TypeSafeClient(new TypeSafeClientOptions
{
    DefaultHeaders = new Dictionary<string, string>
    {
        ["TypeSafe-Danger-Zone-Images"] = "true",
    },
});
```

Per call: `new RequestOptions { Headers = new Dictionary<string, string> { ["TypeSafe-Danger-Zone-Images"] = "true" } }`.

## Create an image

| JS | .NET |
| --- | --- |
| `image(bytes \| ArrayBuffer \| Blob)` | `new Image(byte[])`, `new Image(ReadOnlyMemory<byte>)`, `new Image(Stream)` |
| `Image.fromBase64(text)` | `Image.FromBase64(text)` |

The API detects PNG (`89 50 4E 47…`) and JPEG (`FF D8 FF`) from magic bytes. Other formats are sent as `application/octet-stream` and the API rejects them.

Empty data and invalid base64 throw <xref:TypeSafe.AI.Sdk.TypeSafeException> immediately.

## Attach images

1. **Place in content** — put an <xref:TypeSafe.AI.Sdk.Image> in `state`, instructions, or criteria. It is sent under its generated reference (`<image:{32 lowercase hex}>`).
2. **`WithAssets`** — C# has no tagged templates. Use <xref:TypeSafe.AI.Sdk.WithAssets> (`WithAssets.Format($"… {photo} …")` or `WithAssets.Format("… {0} …", photo)`, and `WithAssets.Join`). A plain interpolated string does **not** attach the image; missing references throw before send.
3. **Named `Assets`** — `SystemOneRequest.Assets` maps caller-chosen names onto images. Names must appear in the serialized JSON; unused names throw.

These can be combined. An image listed in `Assets` and also placed in content is written as that name.

## Wire format

Requests without images stay `application/json`. Requests with images are `multipart/form-data`:

1. Part `request` — JSON payload, including an `assets` map of `{ "type": "image", "part": "asset-N" }`.
2. Later parts `asset-0`, `asset-1`, … — file bytes, content type from magic bytes.

Image bytes are copied when `SystemOneAsync` is called, so later mutation of the caller's buffer does not change what is sent. `Stream` data is read when the `Image` is constructed.

Box questions are not part of this release; they are tracked separately.
