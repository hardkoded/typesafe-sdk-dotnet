# Errors

Exceptions mirror the JavaScript hierarchy (`TypeSafeError` → `TypeSafeException`, and so on).

| Type | When |
| --- | --- |
| [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) | Validation / configuration |
| [ApiException](/api/TypeSafe.AI.Sdk/ApiException/) | Non-2xx after retries |
| [BadRequestException](/api/TypeSafe.AI.Sdk/BadRequestException/) | 400 |
| [AuthenticationException](/api/TypeSafe.AI.Sdk/AuthenticationException/) | 401 |
| [PermissionDeniedException](/api/TypeSafe.AI.Sdk/PermissionDeniedException/) | 403 |
| [NotFoundException](/api/TypeSafe.AI.Sdk/NotFoundException/) | 404 |
| [UnprocessableEntityException](/api/TypeSafe.AI.Sdk/UnprocessableEntityException/) | 422 |
| [RateLimitException](/api/TypeSafe.AI.Sdk/RateLimitException/) | 429 |
| [InternalServerException](/api/TypeSafe.AI.Sdk/InternalServerException/) | 5xx |
| [ApiConnectionException](/api/TypeSafe.AI.Sdk/ApiConnectionException/) | Transport failure |
| [ApiTimeoutException](/api/TypeSafe.AI.Sdk/ApiTimeoutException/) | Attempt timed out |
| [ApiUserAbortException](/api/TypeSafe.AI.Sdk/ApiUserAbortException/) | Cancelled `CancellationToken` |

Catch more specific types first when you need distinct handling for auth vs rate limits vs timeouts.
