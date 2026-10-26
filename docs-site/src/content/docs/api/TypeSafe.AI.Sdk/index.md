---
title: "TypeSafe.AI.Sdk"
description: "Client, questions, responses, options, images, and errors."
editUrl: false
sidebar:
  label: "TypeSafe.AI.Sdk"
---

[API reference](/api/)

Client, questions, responses, options, images, and errors.

## Classes

| Type | Summary |
| --- | --- |
| [Answer](/api/TypeSafe.AI.Sdk/Answer/) | Base type for a typed System One answer. |
| [ApiConnectionException](/api/TypeSafe.AI.Sdk/ApiConnectionException/) | The request or response-body delivery failed (DNS, TLS, connection closed, etc.). Mirrors JS APIConnectionError. |
| [ApiException](/api/TypeSafe.AI.Sdk/ApiException/) | An unsuccessful HTTP response from the API. Mirrors JS APIError. |
| [ApiTimeoutException](/api/TypeSafe.AI.Sdk/ApiTimeoutException/) | The full response did not arrive within the timeout. A kind of ApiConnectionException. Mirrors JS APITimeoutError. |
| [ApiUserAbortException](/api/TypeSafe.AI.Sdk/ApiUserAbortException/) | The caller cancelled the request through a CancellationToken. Mirrors JS APIUserAbortError. |
| [AuthenticationException](/api/TypeSafe.AI.Sdk/AuthenticationException/) | HTTP 401: authentication failed. |
| [BadRequestException](/api/TypeSafe.AI.Sdk/BadRequestException/) | HTTP 400: the request is invalid. |
| [BoxAnswer](/api/TypeSafe.AI.Sdk/BoxAnswer/) | The location of what a box question describes in the image. |
| [BoxCoordinates](/api/TypeSafe.AI.Sdk/BoxCoordinates/) | A box's edges as fractions of the image's height (top, bottom) and width (left, right). |
| [BoxQuestion](/api/TypeSafe.AI.Sdk/BoxQuestion/) | A question that locates what its instructions describe in the request's image. |
| [ChoiceAnswer](/api/TypeSafe.AI.Sdk/ChoiceAnswer/) | A selected label and its probabilities. |
| [ChoiceQuestion](/api/TypeSafe.AI.Sdk/ChoiceQuestion/) | A question that selects between named alternatives. |
| [ConsoleTypeSafeLogger](/api/TypeSafe.AI.Sdk/ConsoleTypeSafeLogger/) | Default console logger with the [typesafe-sdk] prefix. |
| [Env](/api/TypeSafe.AI.Sdk/Env/) | Environment variable names used when constructing TypeSafeClient. Explicit constructor options take precedence over these values. Empty or whitespace-only values are ignored. |
| [Image](/api/TypeSafe.AI.Sdk/Image/) | A PNG or JPEG image for CancellationToken). Image support is experimental: send the TypeSafe-Danger-Zone-Images: true header to opt in. |
| [InternalServerException](/api/TypeSafe.AI.Sdk/InternalServerException/) | HTTP 5xx: the server failed to handle the request. |
| [LogLevels](/api/TypeSafe.AI.Sdk/LogLevels/) | Supported log level names, from most to least verbose. |
| [MicrosoftLoggingTypeSafeLogger](/api/TypeSafe.AI.Sdk/MicrosoftLoggingTypeSafeLogger/) | Adapts ILogger to ITypeSafeLogger. |
| [ModelCard](/api/TypeSafe.AI.Sdk/ModelCard/) | Metadata for an available model. |
| [ModelsResource](/api/TypeSafe.AI.Sdk/ModelsResource/) | Access to the Models API resource. |
| [NotFoundException](/api/TypeSafe.AI.Sdk/NotFoundException/) | HTTP 404: the resource was not found. |
| [NoulAnswer](/api/TypeSafe.AI.Sdk/NoulAnswer/) | A yes/no answer. The value is the probability of yes, from 0 to 1. |
| [NoulCriteria](/api/TypeSafe.AI.Sdk/NoulCriteria/) | Optional descriptions of the yes and no outcomes for a Noul question. |
| [NoulQuestion](/api/TypeSafe.AI.Sdk/NoulQuestion/) | A yes/no question with optional descriptions for either outcome. |
| [PermissionDeniedException](/api/TypeSafe.AI.Sdk/PermissionDeniedException/) | HTTP 403: access is denied. |
| [Question](/api/TypeSafe.AI.Sdk/Question/) | A TypeSafe question identified by its type field. |
| [RateLimitException](/api/TypeSafe.AI.Sdk/RateLimitException/) | HTTP 429: the rate limit was exceeded. |
| [RequestOptions](/api/TypeSafe.AI.Sdk/RequestOptions/) | Per-call options that override client settings. |
| [RetryCalculator](/api/TypeSafe.AI.Sdk/RetryCalculator/) | Retry delay calculation and cancellable waits. Mirrors JS retry.ts. |
| [RetryPolicy](/api/TypeSafe.AI.Sdk/RetryPolicy/) | Retry configuration. Partial overrides inherit unset fields from the client or SDK defaults. Defaults match the JavaScript SDK. |
| [RuntimeInfo](/api/TypeSafe.AI.Sdk/RuntimeInfo/) | Runtime description written to the X-TypeSafe-Runtime header. |
| [ScoreAnswer](/api/TypeSafe.AI.Sdk/ScoreAnswer/) | An expected score with its rubric and probabilities. |
| [ScoreQuestion](/api/TypeSafe.AI.Sdk/ScoreQuestion/) | A question that assigns a score using an ordered rubric. |
| [SdkVersion](/api/TypeSafe.AI.Sdk/SdkVersion/) | SDK version reported in User-Agent and X-TypeSafe-SDK. |
| [SystemOneRequest](/api/TypeSafe.AI.Sdk/SystemOneRequest/) | State and named questions for CancellationToken). |
| [SystemOneResult](/api/TypeSafe.AI.Sdk/SystemOneResult/) | Answers keyed by question name, with model and usage metadata. |
| [TextWithAssets](/api/TypeSafe.AI.Sdk/TextWithAssets/) | Text with the images interpolated into it, sent together with the text. |
| [TypeSafeClient](/api/TypeSafe.AI.Sdk/TypeSafeClient/) | Client for the TypeSafe AI API. Community .NET port of @typesafe-ai/sdk. |
| [TypeSafeClientOptions](/api/TypeSafe.AI.Sdk/TypeSafeClientOptions/) | Client options. Explicit values take precedence over environment variables, then SDK defaults. |
| [TypeSafeException](/api/TypeSafe.AI.Sdk/TypeSafeException/) | Base class for SDK errors. Mirrors JS TypeSafeError. |
| [TypeSafeResponse&lt;T&gt;](/api/TypeSafe.AI.Sdk/TypeSafeResponse-1/) | Parsed data with its HTTP response and request ID. Mirrors JS WithResponse. |
| [UnprocessableEntityException](/api/TypeSafe.AI.Sdk/UnprocessableEntityException/) | HTTP 422: request validation failed. |
| [Usage](/api/TypeSafe.AI.Sdk/Usage/) | Token usage for a request. |

## Interfaces

| Type | Summary |
| --- | --- |
| [ITypeSafeClient](/api/TypeSafe.AI.Sdk/ITypeSafeClient/) | Client for the TypeSafe AI API. |
| [ITypeSafeLogger](/api/TypeSafe.AI.Sdk/ITypeSafeLogger/) | Log methods accepting a message and structured values. Compatible with console in spirit. |

## Enums

| Type | Summary |
| --- | --- |
| [TypeSafeLogLevel](/api/TypeSafe.AI.Sdk/TypeSafeLogLevel/) | Log verbosity; Off disables logging. Mirrors JS LogLevel. |
