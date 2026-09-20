using TypeSafe.AI.Sdk;

if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(Env.ApiKey)))
{
    Console.Error.WriteLine($"Set {Env.ApiKey} and re-run. Example:");
    Console.Error.WriteLine($"  export {Env.ApiKey}=tsk_...");
    Console.Error.WriteLine("  dotnet run --project samples/TypeSafe.AI.Sdk.Sample");
    return 1;
}

using var client = new TypeSafeClient();

var result = await client.SystemOneAsync(
    new
    {
        document = "I was charged twice. Please fix this ASAP.",
    },
    new Dictionary<string, Question>
    {
        ["category"] = Question.Choice("What is this ticket about?", new Dictionary<string, object?>
        {
            ["billing"] = "Payments, invoicing, refunds",
            ["technical"] = "Bugs, outages, integrations",
            ["other"] = null,
        }),
        ["urgent"] = Question.Noul("Does this convey urgency?"),
    });

var category = result.GetChoice("category");
var urgent = result.GetNoul("urgent");

Console.WriteLine($"model:      {result.Model}");
Console.WriteLine($"category:   {category.Choice} (confidence {category.Confidence:0.00})");
Console.WriteLine($"urgent:     {urgent.Noul:0.00}");
Console.WriteLine($"usage:      {result.Usage.InputTokens} in / {result.Usage.OutputTokens} out");
return 0;
