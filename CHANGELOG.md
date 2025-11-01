# KickLib changelog

[![NuGet](https://img.shields.io/nuget/v/KickLib.svg)](https://www.nuget.org/packages/KickLib)

All notable changes to this project will be documented in this file.

## [1.7.2](https://www.nuget.org/packages/KickLib/1.7.2) - [diff](https://github.com/Bukk94/KickLib/compare/v1.7.1...v1.7.2)
### Changed
- Improved working with HTTP Client to better utilize connection pooling
- Added HTTP transient errors retry policy to API calls
- Added IHttpClientFactory to OAuthGenerator

## [1.7.1](https://www.nuget.org/packages/KickLib/1.7.1) - [diff](https://github.com/Bukk94/KickLib/compare/v1.7.0...v1.7.1)
### Added 
- Added Kicks endpoints - full support for new Kicks endpoints and Leaderboards endpoint
- Added SubscribeToEvent to EventSubscriptions, to allow subscribing to single event
### Changed
- Improved interface docs 

## [1.7.0](https://www.nuget.org/packages/KickLib/1.7.0) - [diff](https://github.com/Bukk94/KickLib/compare/v1.6.2...v1.7.0)
### Added
- Added Delete Message for Unofficial API
- Added `KicksGiftedEvent` webhook event support
### Changed
- Extended `IApiCaller` with `HttpMethod` parameter for `SendAuthenticatedRequestAsync` method.
- Extended `SendMessageAsync` in Unofficial API to return sent message data (like ID and sender)
- Extended Unofficial API `AuthenticationSettings` to allow passing override Bearer token to bypass login flow
- Updated Puppeteer version to 20.x for Unofficial API on .NET 8 and above
### Remove
- Removed `PinnedMessageDeletedEvent` in Kick.Client as this event has no data (and it was confusing users)

## [1.6.2](https://www.nuget.org/packages/KickLib/1.6.2) - [diff](https://github.com/Bukk94/KickLib/compare/v1.6.1...v1.6.2)
### Added
- GetLivestreamAsync overload to get livestream for currently authenticated user
- Add KicksGifted event to Client
### Fixed
- Fixed data deserialization for NewSubscriptionEvent webhook event

## [1.6.1](https://www.nuget.org/packages/KickLib/1.6.1) - [diff](https://github.com/Bukk94/KickLib/compare/v1.6.0...v1.6.1)
### Added
- Extended listened topics for `KickLib.Client`
- Added retry policy for `BrowserClient`
- Added replies_to field to `ChatMessageSentEvent` webhook event
### Fixed
- Fixed listening for `SubscriptionEvent` event

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
