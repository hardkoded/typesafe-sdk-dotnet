// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using TypeSafe.AI.Sdk;
using TypeSafe.AI.Sdk.WallySample;

const string Usage = "Usage: dotnet run --project samples/TypeSafe.AI.Sdk.WallySample -- <scene.png|jpg> <character.png|jpg> [output.png]";

if (args.Length is < 2 or > 3)
{
    Console.Error.WriteLine(Usage);
    Console.Error.WriteLine("Pass your own images: the sample does not ship any.");
    return 1;
}

if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(Env.ApiKey)))
{
    Console.Error.WriteLine($"Set {Env.ApiKey} and re-run. Example:");
    Console.Error.WriteLine($"  export {Env.ApiKey}=tsk_...");
    Console.Error.WriteLine(Usage);
    return 1;
}

var scenePath = args[0];
var characterPath = args[1];
var outputPath = args.Length == 3 ? args[2] : "found.png";

if (!outputPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
    || IsSameFile(outputPath, scenePath)
    || IsSameFile(outputPath, characterPath))
{
    Console.Error.WriteLine("The output must be a new .png file, not one of the input images.");
    return 1;
}

// Images are experimental and need this opt-in header.
using var client = new TypeSafeClient(new TypeSafeClientOptions
{
    DefaultHeaders = new Dictionary<string, string> { ["TypeSafe-Danger-Zone-Images"] = "true" },
});

BoxCoordinates box;
double? confidence;
try
{
    (box, confidence) = await WallyFinder.FindAsync(client, scenePath, characterPath);
    WallyFinder.Highlight(scenePath, box, outputPath);
}
catch (Exception ex) when (ex is TypeSafeException or IOException or InvalidOperationException or UnauthorizedAccessException or ArgumentException)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}

Console.WriteLine($"box:        left {box.Left:0.000}, top {box.Top:0.000}, right {box.Right:0.000}, bottom {box.Bottom:0.000}");
Console.WriteLine($"confidence: {(confidence is { } value ? value.ToString("0.00") : "not reported")}");
Console.WriteLine($"saved:      {Path.GetFullPath(outputPath)}");
return 0;

static bool IsSameFile(string first, string second) =>
    string.Equals(Path.GetFullPath(first), Path.GetFullPath(second), StringComparison.OrdinalIgnoreCase);
