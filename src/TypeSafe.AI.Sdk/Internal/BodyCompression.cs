// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Text;
using ZstdSharp;

namespace TypeSafe.AI.Sdk;

internal static class BodyCompression
{
    /// <summary>Bodies below this many bytes fit in a single TCP segment, so they are sent uncompressed.</summary>
    public const int MinCompressedBodyBytes = 1024;

    public const string ContentEncoding = "zstd";

    public static (byte[] Bytes, bool Compressed) Encode(string json, bool compression) =>
        Encode(Encoding.UTF8.GetBytes(json), compression);

    /// <summary>The request body bytes, zstd-compressed when enabled and large enough.</summary>
    public static (byte[] Bytes, bool Compressed) Encode(byte[] bytes, bool compression)
    {
        if (!compression || bytes.Length < MinCompressedBodyBytes)
        {
            return (bytes, false);
        }

        using var compressor = new Compressor();
        return (compressor.Wrap(bytes).ToArray(), true);
    }
}
