# CLAUDE.md

.NET SDK for the TypeSafe AI API. Port of typesafe-sdk-js.

## Target frameworks

- `src/TypeSafe.AI.Sdk` targets `net10.0` and `netstandard2.0`. Code must compile on both.
- `LangVersion` is `latest` (C# 14). Language features are fine on both targets. BCL APIs are not. Guard net10-only APIs with `#if NET`.
- Polyfills live in `src/TypeSafe.AI.Sdk/Internal`. `IsExternalInit` exists. There is no polyfill for `required`, so do not use it in `src`.
- Tests and samples target `net10.0` only.

## Modern C#

Load the `dotnet-claude-kit:modern-csharp` skill before you write or review C# code. Where the skill and this repo disagree, this repo wins.

- Use `field` for a property with a custom accessor. Do not add a new `_backing` field for it.
- Use `[]` for lists and arrays. Dictionaries keep `new Dictionary<,>()`, because C# 14 has no dictionary expressions.
- Use `is null`, `is not`, and switch expressions.
- Prefer `var`. The `.editorconfig` asks for it, even where the skill suggests an explicit type.
- Seal every class that is not meant for inheritance. Use `ConfigureAwait(false)` in `src`.
- Primary constructors are fine on internal types. Do not add them to public types.

## Public API

The public API is frozen by default. Ask before you:

- turn a public class into a record,
- add a primary constructor to a public type,
- change `set` to `init` on an options class,
- add `required` to a public member.

## Build and style

- The build enforces the `.editorconfig` (`EnforceCodeStyleInBuild`, `TreatWarningsAsErrors`). Fix warnings. Do not suppress them without a reason.
- Keep the file header, file-scoped namespaces, and the `s_` (static) and `_` (private) field prefixes.
- `IDE0055` (whitespace) is only a suggestion in the build. Run `dotnet format --verify-no-changes` before you push.
- Run `dotnet build` and `dotnet test`. Put new tests in `tests/TypeSafe.AI.Sdk.Tests`.
