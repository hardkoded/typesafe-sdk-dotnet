# Questions

System One answers four question shapes. Build them with static helpers on <xref:TypeSafe.AI.Sdk.Question>, then read typed answers from <xref:TypeSafe.AI.Sdk.SystemOneResult>.

| Need | Builder | Answer |
| --- | --- | --- |
| One of a defined set | `Question.Choice` | `choice`, probabilities, confidence |
| Degree on ordered levels | `Question.Score` | `score`, legend, probabilities, confidence |
| Is this true? | `Question.Noul` | `noul` ∈ [0, 1] — no separate confidence |
| Where is this in an image? | `Question.Box` | `box` (fractions 0–1), nullable `confidence` |

See [TypeSafe primitives](https://docs.typesafe.ai/primitives.md) for conceptual background.

## Choice

```csharp
("category", Question.Choice(
    "What is this ticket about?",
    ("billing", null),
    ("technical", "Product bugs, outages, or how-to"),
    ("other", null)))
```

Choice criteria is still a map on the wire. The tuple pairs are converted to that map; pass a `Dictionary<string, object?>` when you already have one. Duplicate labels throw.

```csharp
var answer = response.GetChoice("category");
Console.WriteLine(answer.Choice);
Console.WriteLine(answer.Confidence);
```

## Score

Levels are ordered from low to high. Pass at least two level labels after the instructions.

```csharp
("urgency", Question.Score(
    "How urgent is this?",
    "not urgent",
    "soon",
    "blocking"))
```

```csharp
var answer = response.GetScore("urgency");
Console.WriteLine(answer.Score);
Console.WriteLine(string.Join(" → ", answer.Legend));
```

## Noul

Probability that the statement is true.

```csharp
("is_refund", Question.Noul("Is the customer asking for a refund?"))
```

```csharp
var answer = response.GetNoul("is_refund");
Console.WriteLine(answer.Noul);
```

## Box

Locate what the instructions describe in the request's image. Coordinates are fractions of image height (`top`, `bottom`) and width (`left`, `right`), each from 0 to 1. `confidence` is `null` when the API omits it.

Box questions are experimental. Use the `jev-preview` model and send the `TypeSafe-Danger-Zone-Images: true` header on each request that includes a Box question. See [TypeSafe images](https://docs.typesafe.ai/images). Attaching image bytes (`image()` / assets) is a separate SDK surface; `Question.Box` can be sent on JSON requests.

```csharp
var response = await client.SystemOneAsync(
    new SystemOneRequest
    {
        State = state, // image bytes land in a later API; JSON state is valid today
        Questions = Question.Map(("damage", Question.Box("The damaged area of the product"))),
        Model = "jev-preview",
    },
    new RequestOptions
    {
        Headers = new Dictionary<string, string>
        {
            ["TypeSafe-Danger-Zone-Images"] = "true",
        },
    });

var answer = response.GetBox("damage");
var box = answer.Box;
Console.WriteLine($"{box.Top}, {box.Bottom}, {box.Left}, {box.Right}");
Console.WriteLine(answer.Confidence);
```

## Batching

Independent questions share one request. Prefer one `SystemOneAsync` call over many round trips when the questions do not depend on each other.

```csharp
var response = await client.SystemOneAsync(
    state,
    ("category", Question.Choice("What is this ticket about?",
        ("billing", null),
        ("technical", null),
        ("other", null))),
    ("urgency", Question.Score("How urgent is this?", "not urgent", "soon", "blocking")),
    ("is_refund", Question.Noul("Is the customer asking for a refund?")));
```

For `model`, per-call options, or a cancellation token, pass `Question.Map(...)` to the dictionary overload (or set `SystemOneRequest.Questions`).
