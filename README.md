<p align="center">
<a href="https://www.microsoft.com/net"><img src="https://img.shields.io/badge/-.NET%207.0-blueviolet" style="max-height: 300px;"></a>
<img src="https://img.shields.io/badge/Platform-.NET-lightgrey.svg" style="max-height: 300px;">
<a href="https://github.com/Bukk94/KickLib/blob/master/LICENSE"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" style="max-height: 300px;"></a>
</p>

# About

KickLib is a C# library that allows for interaction with unofficial / undocumented Kick API (https://kick.com) 
and eases implementation for various bots.

# Examples

```csharp
IKickApi kickApi = new KickApi();

var userName = "channelUsername";

// Get information about user
var user = await kickApi.Users.GetUserAsync(userName);

// Get information about channel
var channelInfo = await kickApi.Channels.GetChannelInfoAsync(userName);

// Is user broadcasting / streaming?
var isBroadcasting = await kickApi.Livestream.IsStreamerLiveAsync(userName);

// Gets detailed information about current livestream
var liveInfo = await kickApi.Livestream.GetLivestreamInfoAsync(userName);
```

# Limitations

Currently KickLib is not able to do authenticated calls, because any authentication to Kick's API is very tricky.

# Disclaimer

Kick don't have any official API. All functionality in KickLib was researched and reversed-engineered from their website.
This means that any API can change without announce.

KickLib is meant to be used for education purposes. Don't use it for heavy scraping or other harmful actions against
Kick streaming platform. I don't take responsibility for any KickLib misuse.

Once API is officially released, this library will be adjusted accordingly.

# License

See [MIT License](LICENSE)