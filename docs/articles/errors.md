# Errors

Exceptions mirror the JavaScript hierarchy (`TypeSafeError` → `TypeSafeException`, and so on).

| Type | When |
| --- | --- |
| <xref:TypeSafe.AI.Sdk.TypeSafeException> | Validation / configuration |
| <xref:TypeSafe.AI.Sdk.ApiException> | Non-2xx after retries |
| <xref:TypeSafe.AI.Sdk.BadRequestException> | 400 |
| <xref:TypeSafe.AI.Sdk.AuthenticationException> | 401 |
| <xref:TypeSafe.AI.Sdk.PermissionDeniedException> | 403 |
| <xref:TypeSafe.AI.Sdk.NotFoundException> | 404 |
| <xref:TypeSafe.AI.Sdk.UnprocessableEntityException> | 422 |
| <xref:TypeSafe.AI.Sdk.RateLimitException> | 429 |
| <xref:TypeSafe.AI.Sdk.InternalServerException> | 5xx |
| <xref:TypeSafe.AI.Sdk.ApiConnectionException> | Transport failure |
| <xref:TypeSafe.AI.Sdk.ApiTimeoutException> | Attempt timed out |
| <xref:TypeSafe.AI.Sdk.ApiUserAbortException> | Cancelled `CancellationToken` |

Catch more specific types first when you need distinct handling for auth vs rate limits vs timeouts.
