// Copyright (c) Dario Kondratiuk.
// Licensed under the MIT License.

using Microsoft.Playwright;

namespace TypeSafe.AI.Sdk.MemoryGameSample;

/// <summary>
/// Plays the memozor.com cats-and-kittens memory game. The bot only uses what a player sees: a screenshot of each
/// flipped card, kept in memory. TypeSafe decides whether two screenshots show the same picture.
/// </summary>
internal sealed class MemoryGame(ITypeSafeClient client, IPage page)
{
    private const string GameUrl = "https://www.memozor.com/memory-games/for-seniors-or-elderly/cats-and-kittens";
    private const double MatchThreshold = 0.5;

    // The page flips unmatched cards back after about a second and a half.
    private static readonly TimeSpan s_flipBackDelay = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan s_flipAnimationDelay = TimeSpan.FromMilliseconds(500);

    private readonly Dictionary<int, Image> _seen = [];
    private readonly HashSet<int> _matched = [];
    private readonly HashSet<(int, int)> _rejected = [];

    public int Moves { get; private set; }

    public int Mistakes { get; private set; }

    public async Task PlayAsync()
    {
        await page.GotoAsync(GameUrl);
        await page.ClickAsync("#start");
        await page.Locator("#cards").EvaluateAsync("board => board.scrollIntoView({ block: 'center' })");
        var cardCount = await page.Locator(".card").CountAsync();
        (int, int)? pending = null;

        while (_matched.Count < cardCount)
        {
            if (pending is var (known, partner))
            {
                pending = null;
                await FlipAsync(known);
                await FlipPairAsync(known, partner);
                continue;
            }

            var first = NextUnseen(cardCount);
            var firstImage = await FlipAsync(first);
            var match = await FindMatchAsync(first, firstImage);
            if (match is { } sure)
            {
                await FlipPairAsync(first, sure);
                continue;
            }

            var second = NextUnseen(cardCount);
            var secondImage = await FlipAsync(second);
            match = await FindMatchAsync(second, secondImage);
            if (match is { } other && other != first)
            {
                // The second card matches an older card, so this move is a mistake. Next move completes the pair.
                pending = (other, second);
            }

            await ResolveAsync(first, second);
        }

        await page.GetByText("YOU WIN!").WaitForAsync();
    }

    private int NextUnseen(int cardCount)
    {
        var unseen = Enumerable.Range(0, cardCount).Where(i => !_seen.ContainsKey(i)).Cast<int?>().FirstOrDefault();
        return unseen ?? throw new InvalidOperationException("Every card was seen but some pairs were not matched.");
    }

    private async Task<Image> FlipAsync(int index)
    {
        await page.Locator(".card").Nth(index).ClickAsync();
        await page.Locator($"#card-{index}.upturned").WaitForAsync();
        var card = page.Locator($"#card-{index}");
        await Task.Delay(s_flipAnimationDelay);

        if (!_seen.TryGetValue(index, out var image))
        {
            image = new Image(await card.ScreenshotAsync());
            _seen[index] = image;
        }

        return image;
    }

    private async Task FlipPairAsync(int first, int second)
    {
        await FlipAsync(second);
        await ResolveAsync(first, second);
    }

    /// <summary>Waits for the page to react, then checks whether the two flipped cards stayed face up.</summary>
    private async Task ResolveAsync(int first, int second)
    {
        Moves++;
        await Task.Delay(s_flipBackDelay);

        if (await IsUpturnedAsync(first) && await IsUpturnedAsync(second))
        {
            _matched.Add(first);
            _matched.Add(second);
            Console.WriteLine($"Move {Moves}: cards {first} and {second} match.");
        }
        else
        {
            Mistakes++;
            _rejected.Add((Math.Min(first, second), Math.Max(first, second)));
            Console.WriteLine($"Move {Moves}: cards {first} and {second} do not match.");
        }
    }

    private async Task<bool> IsUpturnedAsync(int index) =>
        (await page.Locator($"#card-{index}").GetAttributeAsync("class"))?.Contains("upturned") == true;

    /// <summary>Asks TypeSafe which already seen, unmatched card shows the same picture, if any.</summary>
    private async Task<int?> FindMatchAsync(int index, Image card)
    {
        var candidates = _seen.Keys
            .Where(other => other != index && !_matched.Contains(other) && !_rejected.Contains((Math.Min(index, other), Math.Max(index, other))))
            .ToList();
        if (candidates.Count == 0)
        {
            return null;
        }

        var result = await client.SystemOneAsync(
            "Two cards of a memory game match when they show the same picture.",
            candidates
                .Select(other => ($"card{other}", (Question)Question.Noul(TextWithAssets.From($"Do {card} and {_seen[other]} show the same picture?"))))
                .ToArray());

        var best = candidates
            .Select(other => (Index: other, Probability: result.GetNoul($"card{other}").Noul))
            .MaxBy(candidate => candidate.Probability);
        return best.Probability >= MatchThreshold ? best.Index : null;
    }
}
