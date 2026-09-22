// Copyright (c) Hardkoded.
// Licensed under the MIT License.

using System.Reflection;

namespace TypeSafe.AI.Sdk;

/// <summary>SDK version reported in <c>User-Agent</c> and <c>X-TypeSafe-SDK</c>.</summary>
public static class SdkVersion
{
    /// <summary>Informational version from MinVer / the assembly, without metadata suffixes.</summary>
    public static string Current { get; } = Resolve();

    private static string Resolve()
    {
        var informational = typeof(SdkVersion).Assembly
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        if (string.IsNullOrWhiteSpace(informational))
        {
            var version = typeof(SdkVersion).Assembly.GetName().Version;
            return version is null ? "0.3.0" : $"{version.Major}.{version.Minor}.{version.Build}";
        }

        var value = informational!;
        var plus = value.IndexOf('+');
        return plus >= 0 ? value.Substring(0, plus) : value;
    }
}
