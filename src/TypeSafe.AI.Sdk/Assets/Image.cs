// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace TypeSafe.AI.Sdk;

/// <summary>
/// A PNG or JPEG image asset for <see cref="TypeSafeClient.SystemOneAsync(SystemOneRequest, RequestOptions?, CancellationToken)"/>.
/// Mirrors JS <c>Image</c> / <c>image()</c>.
/// </summary>
/// <remarks>
/// Place the image in <see cref="SystemOneRequest.State"/> or question content, interpolate it
/// with <see cref="WithAssets"/>, or pass it in <see cref="SystemOneRequest.Assets"/> under a
/// name of your choosing. Image support is experimental: the SDK does not send
/// <c>TypeSafe-Danger-Zone-Images</c> automatically — opt in via
/// <see cref="TypeSafeClientOptions.DefaultHeaders"/> or <see cref="RequestOptions.Headers"/>.
/// </remarks>
[JsonConverter(typeof(ImageJsonConverter))]
public sealed class Image
{
    /// <summary>Matches <see cref="ToString"/>: the reference an image is sent under.</summary>
    internal static readonly Regex s_referencePattern = new(
        "<image:[0-9a-f]{32}>",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly byte[] s_pngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
    private static readonly byte[] s_jpegSignature = { 0xFF, 0xD8, 0xFF };

    private readonly byte[] _data;
    private readonly int _offset;
    private readonly int _length;
    private readonly string _reference;

    /// <summary>Always <c>image</c>. Mirrors JS <c>Image.type</c>.</summary>
    public string Type => "image";

    /// <summary>The image file bytes. Mutations to a caller-owned buffer are visible until the request is serialized.</summary>
    public ReadOnlyMemory<byte> Data => new(_data, _offset, _length);

    /// <summary>
    /// Create an image from PNG or JPEG file bytes.
    /// Mirrors JS <c>image(bytes)</c>.
    /// </summary>
    /// <param name="data">Image file bytes. Stored without copying; copied when the request is serialized.</param>
    /// <exception cref="TypeSafeException"><paramref name="data"/> is null or empty.</exception>
    public Image(byte[] data)
        : this(data, offset: 0, length: data is null ? 0 : data.Length)
    {
    }

    /// <summary>
    /// Create an image from PNG or JPEG file bytes.
    /// Mirrors JS <c>image(ArrayBufferView)</c>: only the viewed range is sent.
    /// </summary>
    /// <exception cref="TypeSafeException"><paramref name="data"/> is empty.</exception>
    public Image(ReadOnlyMemory<byte> data)
    {
        if (data.IsEmpty)
        {
            throw new TypeSafeException("Image data is empty.");
        }

        if (System.Runtime.InteropServices.MemoryMarshal.TryGetArray(data, out var segment)
            && segment.Array is not null)
        {
            _data = segment.Array;
            _offset = segment.Offset;
            _length = segment.Count;
        }
        else
        {
            _data = data.ToArray();
            _offset = 0;
            _length = _data.Length;
        }

        _reference = CreateReference();
    }

    /// <summary>
    /// Create an image by reading PNG or JPEG file bytes from a stream now.
    /// The caller retains ownership of <paramref name="data"/> and may dispose it after this returns.
    /// </summary>
    /// <exception cref="TypeSafeException"><paramref name="data"/> is null or empty.</exception>
    public Image(Stream data)
    {
        if (data is null)
        {
            throw new TypeSafeException("Image data must be file bytes.");
        }

        using var copy = new MemoryStream();
        data.CopyTo(copy);
        if (copy.Length == 0)
        {
            throw new TypeSafeException("Image data is empty.");
        }

        _data = copy.ToArray();
        _offset = 0;
        _length = _data.Length;
        _reference = CreateReference();
    }

    private Image(byte[] data, int offset, int length)
    {
        if (data is null)
        {
            throw new TypeSafeException("Image data must be file bytes.");
        }

        if (length == 0)
        {
            throw new TypeSafeException("Image data is empty.");
        }

        _data = data;
        _offset = offset;
        _length = length;
        _reference = CreateReference();
    }

    /// <summary>
    /// Create an image from base64-encoded file bytes, decoding them now.
    /// Mirrors JS <c>Image.fromBase64</c>.
    /// </summary>
    /// <param name="text">The image file bytes, base64-encoded, without a <c>data:</c> URL prefix.</param>
    /// <exception cref="TypeSafeException"><paramref name="text"/> is not valid base64 or decodes to no bytes.</exception>
    public static Image FromBase64(string text)
    {
        byte[] binary;
        try
        {
            binary = Convert.FromBase64String(text ?? "");
        }
        catch (FormatException ex)
        {
            throw new TypeSafeException("Image data must contain valid base64 text.", ex);
        }

        return new Image(binary);
    }

    /// <summary>The unique reference this image is sent under, for referencing it inside text.</summary>
    public override string ToString() => _reference;

    internal byte[] CopyBytes()
    {
        var copy = new byte[_length];
        Array.Copy(_data, _offset, copy, 0, _length);
        return copy;
    }

    internal static string ContentTypeOf(byte[] bytes)
    {
        if (StartsWith(bytes, s_pngSignature))
        {
            return "image/png";
        }

        if (StartsWith(bytes, s_jpegSignature))
        {
            return "image/jpeg";
        }

        return "application/octet-stream";
    }

    private static bool StartsWith(byte[] bytes, byte[] signature)
    {
        if (bytes.Length < signature.Length)
        {
            return false;
        }

        for (var i = 0; i < signature.Length; i++)
        {
            if (bytes[i] != signature[i])
            {
                return false;
            }
        }

        return true;
    }

    private static string CreateReference() => "<image:" + RandomHex() + ">";

    private static string RandomHex()
    {
        var bytes = new byte[16];
#if NET
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToHexString(bytes).ToLowerInvariant();
#else
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        var chars = new char[32];
        for (var i = 0; i < bytes.Length; i++)
        {
            var value = bytes[i];
            chars[i * 2] = ToHexChar(value >> 4);
            chars[(i * 2) + 1] = ToHexChar(value & 0xF);
        }

        return new string(chars);
#endif
    }

#if !NET
    private static char ToHexChar(int nibble) =>
        (char)(nibble < 10 ? '0' + nibble : 'a' + (nibble - 10));
#endif
}

internal sealed class ImageJsonConverter : JsonConverter<Image>
{
    private readonly AssetWriteContext? _context;

    public ImageJsonConverter()
        : this(null)
    {
    }

    public ImageJsonConverter(AssetWriteContext? context)
    {
        _context = context;
    }

    public override Image Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException("Image values cannot be deserialized from JSON.");
    }

    public override void Write(Utf8JsonWriter writer, Image value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_context is null ? value.ToString() : _context.WriteImage(value));
    }
}
