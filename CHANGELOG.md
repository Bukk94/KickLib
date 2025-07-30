# KickLib changelog

[![NuGet](https://img.shields.io/nuget/v/KickLib.svg)](https://www.nuget.org/packages/KickLib)

All notable changes to this project will be documented in this file.

## [1.6.0](https://www.nuget.org/packages/KickLib/1.6.0) - [diff](https://github.com/Bukk94/KickLib/compare/v1.5.1...v1.6.0)
### Added
- Support for `.NET Standard 2.1`
- Extended `GetLivestreamsAsync` to get livestreams by multiple `broadcasterIds`

## [1.5.1](https://www.nuget.org/packages/KickLib/1.5.1) - [diff](https://github.com/Bukk94/KickLib/compare/v1.5.0...v1.5.1)
### Added
- Reply message metadata to `KickLib.Client` Message data
- `CreatedAt` field to `ChatMessageSentEvent` webhook event

## [1.5.0](https://www.nuget.org/packages/KickLib/1.5.0) - [diff](https://github.com/Bukk94/KickLib/compare/v1.4.4...v1.5.0)
### Added
- Session-based browser client for unofficial API

### Changed
- Added proper handling for Unofficial API, when request fails due to Cloudflare protection
- Safe parsing of the authentication response to prevent exceptions when the response is not in the expected format.

## [1.4.3](https://www.nuget.org/packages/KickLib/1.4.3) - [diff](https://github.com/Bukk94/KickLib/compare/v1.4.2...v1.4.3)
### Added
- Added `KickLibRefreshTokenException` that is fired, when refresh token fails to refresh (usually because token expired or is invalid)
- Extended all Result classes with `KickLibHttpResponseError` object, which contains error details if the request failed, including original HTTP response message
- Added `GetUserAsync` to `Users` API to get single user by ID

## [1.4.2](https://www.nuget.org/packages/KickLib/1.4.2) - [diff](https://github.com/Bukk94/KickLib/compare/v1.4.1...v1.4.2)
### Changed
- Rolled back PuppeteerSharp version from 20.x to 14.0.0
  - This was done to ensure compatibility with `PuppeteerExtraSharp` library 

## [1.4.1](https://www.nuget.org/packages/KickLib/1.4.1) - [diff](https://github.com/Bukk94/KickLib/compare/v1.4.0...v1.4.1)
### Added
- Extended KickClient with `OnRewardRedeemed` event

## [1.4.0](https://www.nuget.org/packages/KickLib/1.4.0) - [diff](https://github.com/Bukk94/KickLib/compare/v1.3.0...v1.4.0)
### Added
- Support for new webhook Event Type `moderation.banned`
- Support for chat moderation endpoints
- Missing docs
- New `moderation:ban` scope
### Changed
- Fixed project warnings
- Updated README
- Updated NuGet packages to latest versions

## [1.3.0](https://www.nuget.org/packages/KickLib/1.3.0) - [diff](https://github.com/Bukk94/KickLib/compare/v1.2.2...v1.3.0)
### Changed
- KickLib constructor was changed to fit Dependency Injection pattern
  - To use "simple" constructor, use `KickApi.Create()` method
### Removed
- .NET 7 support

## [1.2.2](https://www.nuget.org/packages/KickLib/1.2.2) - [diff](https://github.com/Bukk94/KickLib/compare/v1.2.1...v1.2.2)
### Added
- Support for message reply (`ReplyToMessageAsUserAsync`, `ReplyToMessageAsBotAsync`)
- LivestreamMetadataUpdatedEvent webhook support
- More unit tests covering new webhook types

## [1.2.1](https://www.nuget.org/packages/KickLib/1.2.1) - [diff](https://github.com/Bukk94/KickLib/compare/v1.2.0...v1.2.1)
### Added
- Search channels by `slug`
- `GetChannelAsync` overloads
- Thumbnail to Stream response
- Paging for `GetCategoriesAsync` endpoint

## [1.2.0](https://www.nuget.org/packages/KickLib/1.2.0) - [diff](https://github.com/Bukk94/KickLib/compare/v1.1.1...v1.2.0)
### Added
- Support for the `v1/livestreams` API.
- Support for `UserIdentity` (badges) in `ChatMessageSentEvent` webhook payload
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
