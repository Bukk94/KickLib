<p align="center"> 
<img src="KickLibLogo.png" style="max-height: 300px;">
</p>

<p align="center">
<a href="https://www.microsoft.com/net"><img src="https://img.shields.io/badge/-.NET%209.0-blueviolet" style="max-height: 300px;"></a>
<img src="https://img.shields.io/badge/Platform-.NET-lightgrey.svg" style="max-height: 300px;">
<a href="https://discord.gg/fPRXy57WrS"><img src="https://img.shields.io/badge/Discord-KickLib-green.svg" style="max-height: 300px;"></a>
<a href="https://github.com/Bukk94/KickLib/blob/master/LICENSE"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" style="max-height: 300px;"></a>
<a href="https://www.nuget.org/packages/KickLib"><img src="https://img.shields.io/nuget/dt/KickLib?label=NuGet&color=orange" style="max-height: 300px;"></a>
</p>

<p align="center">
  <a href='https://ko-fi.com/bukk94' target='_blank'>
  <img height='30' style='border:0;height:38px;' src='https://storage.ko-fi.com/cdn/kofi6.png?v=6' border='0' alt='Buy Me a Coffee at ko-fi.com' />
</a>

# About

KickLib is a C# library that allows for interaction with both official and unofficial (undocumented) Kick API (https://kick.com) 
 and WebSocket. KickLib eases implementation for various chatbots by providing simple to use methods.

## KickLib Highlights ✨

* OAuth 2.1 flow
* Real-time chat reading
* Stream state detection
* Authentication flow
* Message sending
* API Endpoint calls

<details>
<summary>Click here to see Complete Features List</summary>

### Client
* Reading Chatroom events
  * New message received
  * Message deleted
  * User banned / unbanned
  * New subscriptions
  * Subscriptions gifts
  * Stream host changes
  * New pinned message
  * Pinned message deleted
* Reading Channel events 
  * Followers status updated
  * Stream state detection
  * Gifts leaderboards updated

### API
* Categories
  * Get all main (root) categories
  * Get specific main category
  * Get top categories
  * Get sub-categories (paged)
  * Get all sub-categories (list all)
  * Get specific sub-category
  * Get subcategory clips (paged)
* Clips
  * Get all Kick clips
  * Get clip information
  * Download clip
* Channels
  * Get messages
  * Get channel information
  * Get channel chatroom information
  * Get channel chatroom rules
  * Get channel polls
  * Get channel clips
  * Get channel links
  * Get channel videos
  * Get channel latest video
  * Get channel leaderboards
  * Get latest subscriber (Requires Authentication)
  * Get followers count
* Emotes
  * Get channel emotes
* Livestreams
  * Is streamer live?
  * Get livestream information 
* Message
  * Send message to chatroom (Requires Authentication)
* Users
  * Get user information
* Videos
  * Get video
</details>

## Unofficial API support

KickLib provides support for unofficial API calls via `IKickUnofficialApi` interface.
Documentation for unofficial API can be found [here](KickLib.Api.Unofficial/README_UnofficialAPI.md).

## Installing ⏫

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). 
Then, install [KickLib](https://www.nuget.org/packages/KickLib) from the package manager console:

```
PM> Install-Package KickLib
```
Or from the .NET CLI as:
```
dotnet add package KickLib
```

### Using KickLib via Dependency Injection

If you are using Dependency Injection, you can easily add KickLib via extension method 
`.AddKickLib()`, that will register all related services with Scoped lifetime.

## Examples 💡

### OAuth flow

Simple OAuth flow using KickLib OAuth generator.

> NOTE: If no state is provided, it will automatically generate state for you as base64 encoded verifier code! 

```csharp
var callbackUrl = "https://localhost:5001/auth/kick/callback"; 
var clientId = "01AAAAA0EXAMPLE66YG2HD9R";
var clientSecret = "aaac0000EXAMPLE8ebe4dc223d0c45187";
var url = KickOAuthGenerator.GetAuthorizationUri(
  callbackUrl, 
  clientId, 
  new List<string>
  {
      "user:read",
      "channel:read"
  }, out var verifier);

// TODO: Perform URL redirect for user and pass OAuth process
// Via callback URL, you will receive code and state
 
var code = "NAAAAAAY5YZQ00STATE000ZZTFHM2I1NJLK";
var state = "ZXhhbXBsZSB2YWx1ZQ=="; 
var exchangeResults = await KickOAuthGenerator.ExchangeCodeForTokenAsync(
    state,
    clientId,
    clientSecret,
    callbackUrl,
    state);
```

### Refreshing Access Token

```csharp
var clientId = "01AAAAA0EXAMPLE66YG2HD9R";
var clientSecret = "aaac0000EXAMPLE8ebe4dc223d0c45187";
var refreshToken = "NAAAAAAY5YZQ00REFRESHTOKEN000ZZTFHM2I1NJLK";
var exchangeResults = await KickOAuthGenerator.RefreshAccessTokenAsync(
    refreshToken,
    clientId,
    clientSecret);
```
  
### Using API to get information
```csharp
IKickApi api = new KickApi();

// Get specific category by ID
var category = await kickApi.Categories.GetCategoryAsync(28, accessToken);
```

### Using Client to read chat messages

```csharp
IKickClient client = new KickClient();

client.OnMessage += delegate(object sender, ChatMessageEventArgs e)
{
    Console.WriteLine(e.Data.Content);
};

await client.ListenToChatRoomAsync(123456);
await client.ConnectAsync();
```

### Authenticated API calls

All calls require authentication. Kick officially supports OAuth 2.1 flow.

KickLib provides tools for generating OAuth URLs, exchanging code for tokens, and refreshing tokens.

To perform calls with authentication, set `AccessToken` in settings object or pass it via method call.
```csharp
IKickApi api = new KickApi(new ApiSettings
{
    AccessToken = accessToken
}, logger);

var category = await kickApi.Categories.GetCategoryAsync(28);
```

# Disclaimer

For a long time, Kick didn't have any official API.
Most of the functionality in KickLib was researched and reversed-engineered from their website.

With new released API support, this library will be adjusted accordingly, removing all unofficial endpoints and methods.
Those methods will be replaced with proper official API calls (once we have all of them).

KickLib is meant to be used for education purposes. Don't use it for heavy scraping or other harmful actions against
Kick streaming platform. I don't take responsibility for any KickLib misuse and I strongly advice against such actions.

# Special Thanks

@Robertsmania for helping with OTP generation, library improvements, and extensive testing.

# License

See [MIT License](LICENSE).