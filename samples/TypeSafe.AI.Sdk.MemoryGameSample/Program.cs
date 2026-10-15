// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using Microsoft.Playwright;
using TypeSafe.AI.Sdk;
using TypeSafe.AI.Sdk.MemoryGameSample;

if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(Env.ApiKey)))
{
    Console.Error.WriteLine($"Set {Env.ApiKey} and re-run. Example:");
    Console.Error.WriteLine($"  export {Env.ApiKey}=tsk_...");
    Console.Error.WriteLine("  dotnet run --project samples/TypeSafe.AI.Sdk.MemoryGameSample");
    Console.Error.WriteLine("The first run also needs a browser; see the Sample section of the README.");
    return 1;
}

// Images are experimental and need this opt-in header.
using var client = new TypeSafeClient(new TypeSafeClientOptions
{
    DefaultHeaders = new Dictionary<string, string> { ["TypeSafe-Danger-Zone-Images"] = "true" },
});

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
var page = await browser.NewPageAsync(new BrowserNewPageOptions
{
    ViewportSize = new ViewportSize { Width = 1280, Height = 1000 },

    // A card is about 128 CSS pixels wide, and the API rejects images under about 300 pixels ("Invalid image dimensions").
    DeviceScaleFactor = 3,
});

var game = new MemoryGame(client, page);
await game.PlayAsync();

Console.WriteLine($"Moves:    {game.Moves}");
Console.WriteLine($"Mistakes: {game.Mistakes}");
return 0;
