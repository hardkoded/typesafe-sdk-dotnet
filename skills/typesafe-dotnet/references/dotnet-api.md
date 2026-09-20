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
- `SystemOneResult.GetChoice` / `GetScore` / `GetNoul` / `Get<T>`
- `RetryPolicy`, `RequestOptions`, `TypeSafeClientOptions`, `Env`, `SdkVersion`

## Headers sent

`Authorization: Bearer …`, `Accept: application/json`, `User-Agent` / `X-TypeSafe-SDK: typesafe-sdk-dotnet/{version}`, `X-TypeSafe-Runtime: dotnet/{version} ({os}; {arch})`, `X-TypeSafe-Retry-Count` on retries.

## Endpoints

- `POST {base}/v1/systemone`
- `GET {base}/v1/models`
