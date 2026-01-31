# Migration Guide for Breaking Changes

This document provides a guide for migrating from one (major/minor) version of the KickLib to another when breaking changes are introduced.

## Table of Contents

- [1.9.0 -> 1.10.0](#190---1100)
- [1.6.x -> 1.7.0](#16x---170)
- [1.2.0 -> 1.3.0](#12x---130)
- [1.1.1 -> 1.2.0](#111---120)
- [0.2.0 -> 1.0.0](#020---100)
- [0.1.x -> 0.2.0](#01x---020)

## 1.9.0 -> 1.10.0

* Kick has deprecated v1 categories endpoints in favor of better v2. 
This means that old method allowing pagination via page number and filter by keyword will be soon removed.
  * To migrate, use new `GetCategoriesAsync` method overloads that support cursor-based pagination and various filtering options.
* Kick also stopped sending `viewer_count` for categories, so this property will always be `null` now.

## 1.6.x -> 1.7.0

* `IApiCaller` in Unofficial API was changed. Method `SendAuthenticatedRequestAsync` was extended with another parameter (`HttpMethod? method = null`).
If you have custom implementation, you need to update it accordingly.

## 1.2.x -> 1.3.0

* .NET 7 support was removed.
* Constructor of `KickApi` was changed to fit Dependency Injection pattern.
  * To use "simple" constructor, use `KickApi.Create()` method.
* `KickOAuthGenerator` is no longer `static`. To use its methods, you need to create an instance of it.

## 1.1.1 -> 1.2.0

* `Api.Authorization.GetPublicKeyAsync()` no longer needs access token.

## 0.2.0 -> 1.0.0

This version introduces a new, **official API for Kick**. There are huge breaking changes as most of the structures changed.
Unofficial API is still available, but it was moved and renamed. I recommend using the official API.

### Unofficial API changes

Old API remains available, but it was moved to `KickLib.Api.Unofficial` namespace. 
It was also renamed from `IKickApi` to `IUnofficialKickApi` (including implementation `KickApi` to `UnofficialKickApi`).

From now on, `KickApi` will be primarily used for the official API.

### Namespace changes

All structures for Unofficial API were moved to `KickLib.Api.Unofficial` namespace.

## 0.1.x -> 0.2.0

### Namespace changes

- Clients classes (Puppeteer/TlsSpoofing) were moved to namespace `KickLib.Clients`
- Specific clients implementation and structures were moved to their respective namespaces:
  - `KickLib.Clients.Puppeteer`
  - `KickLib.Clients.CycleTls`

### Changes in `KickLib` DI registration

Instead of registering everything via `AddKickLib`, KickLib has not builder to attach
specific clients. This allows for more flexibility and control over the client implementation.

You can use `WithTlsClient` or `WithPuppeteerClient` to register specific client implementation.

```csharp
serviceCollection
  .AddKickLib()
  .WithPuppeteerClient();
```

### End of .NET 6 support

Starting from version 0.2.0, KickLib will no longer support .NET 6.

### End of .NET 7 support

Starting from version 1.3.0, KickLib will no longer support .NET 7.