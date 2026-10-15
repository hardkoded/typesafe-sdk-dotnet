# TypeSafe.AI.Sdk API cheat sheet

Package: `TypeSafe.AI.Sdk`  
Namespace: `TypeSafe.AI.Sdk`  
DI: `TypeSafe.AI.Sdk.DependencyInjection.AddTypeSafeClient`

## Types

- `TypeSafeClient` / `ITypeSafeClient`
  - `SystemOneAsync(SystemOneRequest | state+questions, RequestOptions?, CancellationToken)`
  - `SystemOneWithResponseAsync` → `TypeSafeResponse<T>` (`Data`, `Response`, `RequestId`)
  - `Models.ListAsync` / `ListWithResponseAsync`
- `Question.Choice` / `Question.Score` / `Question.Noul` (Choice criteria: tuple pairs or a dictionary)
- `Question.Map` — name/question pairs → `Dictionary<string, Question>`
- `Image` / `Image.FromBase64` — PNG/JPEG asset (JS `image()` / `Image.fromBase64`)
- `WithAssets.Format` / `WithAssets.Join` — JS `withAssets` / `withAssets.join` (no tagged templates in C#)
- `TextWithAssets` — text plus attached images
- `SystemOneRequest.Assets` — `IDictionary<string, Image>` of caller-chosen names
- `SystemOneResult.GetChoice` / `GetScore` / `GetNoul` / `Get<T>`
- `RetryPolicy`, `RequestOptions`, `TypeSafeClientOptions`, `Env`, `SdkVersion`

Image support is experimental. Do not auto-set `TypeSafe-Danger-Zone-Images`; callers opt in via `DefaultHeaders` or `RequestOptions.Headers`. Requests with images are `multipart/form-data`.

## Headers sent

`Authorization: Bearer …`, `Accept: application/json`, `User-Agent` / `X-TypeSafe-SDK: typesafe-sdk-dotnet/{version}`, `X-TypeSafe-Runtime: dotnet/{version} ({os}; {arch})`, `X-TypeSafe-Retry-Count` on retries.

## Endpoints

- `POST {base}/v1/systemone`
- `GET {base}/v1/models`
