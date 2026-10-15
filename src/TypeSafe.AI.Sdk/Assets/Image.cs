// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Globalization;

namespace TypeSafe.AI.Sdk;

/// <summary>
/// A PNG or JPEG image for <see cref="TypeSafeClient.SystemOneAsync(SystemOneRequest, RequestOptions?, CancellationToken)"/>.
/// Image support is experimental: send the <c>TypeSafe-Danger-Zone-Images: true</c> header to opt in.
/// </summary>
/// <remarks>
/// Place the image in <see cref="SystemOneRequest.State"/> or in question instructions or criteria, or interpolate it
/// into <see cref="TextWithAssets.From(FormattableString)"/>; either way it is sent under its own reference,
/// <see cref="ToString"/>. To reference it by a name of your choice, add it to <see cref="SystemOneRequest.Assets"/>.
/// The API detects the format from the file bytes and rejects formats other than PNG and JPEG with a
/// <see cref="BadRequestException"/>.
/// </remarks>
public sealed class Image
{
    private readonly string _reference = $"<image:{Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture)}>";
    private readonly byte[] _bytes;

    /// <summary>Create an image from file bytes. The bytes are copied.</summary>
    /// <exception cref="TypeSafeException"><paramref name="data"/> is empty.</exception>
    public Image(ReadOnlyMemory<byte> data)
    {
        if (data.IsEmpty)
        {
            throw new TypeSafeException("Image data is empty.");
        }

        _bytes = data.ToArray();
    }

    /// <summary>The image file bytes.</summary>
    public ReadOnlyMemory<byte> Data => _bytes;

    internal byte[] Bytes => _bytes;

    /// <summary>Create an image from base64-encoded file bytes, without a <c>data:</c> URL prefix.</summary>
    /// <exception cref="TypeSafeException"><paramref name="text"/> isn't valid base64 or decodes to no bytes.</exception>
    public static Image FromBase64(string text)
    {
        try
        {
            return new Image(Convert.FromBase64String(text));
        }
        catch (FormatException ex)
        {
            throw new TypeSafeException("Image data must contain valid base64 text.", ex);
        }
    }

    /// <summary>Create an image from the bytes of a file, read now.</summary>
    /// <exception cref="TypeSafeException">The file is empty.</exception>
    public static Image FromFile(string path) => new(File.ReadAllBytes(path));

    /// <summary>The unique reference this image is sent under, for referencing it inside text.</summary>
    public override string ToString() => _reference;
}
