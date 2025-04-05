# KickLib changelog

[![NuGet](https://img.shields.io/nuget/v/KickLib.svg)](https://www.nuget.org/packages/KickLib)

All notable changes to this project will be documented in this file.

## [1.2.0](https://www.nuget.org/packages/KickLib/1.2.0) - [diff](https://github.com/Bukk94/KickLib/compare/v1.1.1...v1.2.0)
### Added
- Support for the `v1/livestreams` API.
- Extended subscription webhook payload events with `ExpiresAt`
- Added `CancellationToken` support to all API methods.

### Changed
- `Api.Authorization.GetPublicKeyAsync()` no longer requires an access token.
- Reduced projects dependencies

---

## [1.1.1](https://www.nuget.org/packages/KickLib/1.1.1) - [diff](https://github.com/Bukk94/KickLib/compare/v1.1.0...v1.1.1)
### Fixed
- Deserialization issue with the `message_id` field in webhook events.

---

## [1.1.0](https://www.nuget.org/packages/KickLib/1.1.0) - [diff](https://github.com/Bukk94/KickLib/compare/v1.0.3...v1.1.0)
### Added
- App Access Token support.

### Changed
- Documentation improvements.

---

## [1.0.3](https://www.nuget.org/packages/KickLib/1.0.3) - [diff](https://github.com/Bukk94/KickLib/compare/v1.0.2...v1.0.3)
### Added
- Parameterless constructor for the `KickApi` class.
- `TokenChanged` events in `ApiSettings` for access/refresh token updates.

### Changed
- Improved User-Agent randomization for unofficial clients (legacy unofficial clients).

---

## [1.0.2](https://www.nuget.org/packages/KickLib/1.0.2) - [diff](https://github.com/Bukk94/KickLib/compare/v1.0.0...v1.0.2)
### Added
- Automatic access token refresh when expired (if refresh credentials are provided).
- Additional method overloads for improved usability.

---

## [1.0.0](https://www.nuget.org/packages/KickLib/1.0.0)
### Added
- Initial release of the official Kick API client.
