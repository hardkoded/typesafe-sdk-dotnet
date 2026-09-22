// Copyright (c) Hardkoded.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace TypeSafe.AI.Sdk;

/// <summary>Runtime description written to the <c>X-TypeSafe-Runtime</c> header.</summary>
public static class RuntimeInfo
{
    /// <summary>Cached <c>dotnet/{version} ({os}; {arch})</c> identifier.</summary>
    public static string Description { get; } = Describe();

    private static string Describe()
    {
        var version = Environment.Version.ToString();
        var os = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "win32"
            : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "darwin"
            : RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "linux"
            : RuntimeInformation.OSDescription.Split(' ')[0].ToLowerInvariant();
        var arch = RuntimeInformation.OSArchitecture.ToString().ToLowerInvariant();
        return $"dotnet/{version} ({os}; {arch})";
    }
}
