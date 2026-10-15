// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

namespace TypeSafe.AI.Sdk;

/// <summary>
/// Request body compression, sent as <c>Content-Encoding</c>.
/// Mirrors JS <c>Compression</c> (<c>"zstd"</c> or <c>false</c>) plus an Auto default.
/// </summary>
public enum RequestCompression
{
    /// <summary>
    /// Use zstd when the runtime can compress with it, otherwise send bodies uncompressed.
    /// This is the client default and matches omitting <c>compression</c> in the JS SDK.
    /// </summary>
    Auto = 0,

    /// <summary>
    /// Send all bodies uncompressed. Mirrors JS <c>compression: false</c>.
    /// </summary>
    None = 1,

    /// <summary>
    /// Compress bodies of 1 KiB or more with zstd.
    /// Throws <see cref="TypeSafeException"/> if zstd is unavailable.
    /// Mirrors JS <c>compression: "zstd"</c>.
    /// </summary>
    Zstd = 2,
}
