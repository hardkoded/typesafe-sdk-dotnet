// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using System.Text;
using ZstdSharp;

namespace TypeSafe.AI.Sdk;

/// <summary>
/// Resolves <see cref="RequestCompression"/> and compresses JSON request bodies with zstd.
/// zstd is provided by ZstdSharp.Port on both <c>net10.0</c> and <c>netstandard2.0</c>;
/// the BCL has no zstd APIs until .NET 11.
/// </summary>
internal static class RequestCompressor
{
    /// <summary>Bodies below this many bytes fit in a single TCP segment, so they are sent uncompressed.</summary>
    internal const int MinCompressedBodyBytes = 1024;

    /// <summary>Value written to <c>Content-Encoding</c> when a body is compressed.</summary>
    internal const string ContentEncoding = "zstd";

    private static readonly AsyncLocal<bool?> s_availabilityOverride = new();

    /// <summary>Whether this runtime can compress with zstd. Overridable in tests.</summary>
    internal static bool IsAvailable => s_availabilityOverride.Value ?? true;

    internal static IDisposable OverrideAvailability(bool available)
    {
        var previous = s_availabilityOverride.Value;
        s_availabilityOverride.Value = available;
        return new AvailabilityScope(() => s_availabilityOverride.Value = previous);
    }

    /// <summary>
    /// Resolve the compression option to the encoding request bodies are sent with:
    /// zstd when the runtime has it, otherwise <see cref="RequestCompression.None"/>.
    /// </summary>
    /// <exception cref="TypeSafeException">The value isn't a supported option, or the runtime lacks it.</exception>
    internal static RequestCompression Resolve(RequestCompression compression)
    {
        switch (compression)
        {
            case RequestCompression.Auto:
                return IsAvailable ? RequestCompression.Zstd : RequestCompression.None;
            case RequestCompression.None:
                return RequestCompression.None;
            case RequestCompression.Zstd:
                if (!IsAvailable)
                {
                    throw new TypeSafeException(
                        "zstd compression is not available in this runtime. Pass Compression = RequestCompression.None instead.");
                }

                return RequestCompression.Zstd;
            default:
                throw new TypeSafeException(
                    $"`compression` must be Auto, Zstd, or None, got {compression}.");
        }
    }

    /// <summary>The size of a request body in UTF-8 bytes.</summary>
    internal static int ByteLength(string body) => Encoding.UTF8.GetByteCount(body);

    internal static byte[] Compress(byte[] body)
    {
        using var compressor = new Compressor();
        return compressor.Wrap(body).ToArray();
    }

    internal static byte[] Decompress(byte[] body)
    {
        using var decompressor = new Decompressor();
        return decompressor.Unwrap(body).ToArray();
    }

    private sealed class AvailabilityScope : IDisposable
    {
        private readonly Action _reset;
        private bool _disposed;

        public AvailabilityScope(Action reset)
        {
            _reset = reset;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _reset();
        }
    }
}
