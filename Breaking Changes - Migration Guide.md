# Migration Guide for Breaking Changes

This document provides a guide for migrating from one (major/minor) version of the KickLib to another when breaking changes are introduced.

## Table of Contents

- [0.2.0 -> 1.0.0](#020---100)
- [0.1.x -> 0.2.0](#01x---020)

## 0.2.0 -> 1.0.0

This version introduces a new, **official API for Kick**. There are huge breaking changes as most of the structures changed.
Unofficial API is still available, but it was moved and renamed. I recommend using the official API.

### Unofficial API changes

Old API remains available, but it was moved to `KickLib.Unofficial` namespace. 
It was also renamed from `IKickApi` to `IUnofficialKickApi` (including implementation `KickApi` to `UnofficialKickApi`).

From now on, `KickApi` will be primarily used for the official API.

### Namespace changes

All structures for Unofficial API were moved to `KickLib.Unofficial` namespace.

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