<p align="center"> 
<img src="KickLibLogo.png" style="max-height: 300px;">
</p>

<p align="center">
<a href="https://www.microsoft.com/net"><img src="https://img.shields.io/badge/-.NET%207.0-blueviolet" style="max-height: 300px;"></a>
<img src="https://img.shields.io/badge/Platform-.NET-lightgrey.svg" style="max-height: 300px;">
<a href="https://discord.gg/fPRXy57WrS"><img src="https://img.shields.io/badge/Discord-KickLib-green.svg" style="max-height: 300px;"></a>
<a href="https://github.com/Bukk94/KickLib/blob/master/LICENSE"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" style="max-height: 300px;"></a>
</p>

<p align="center">
  <a href='https://ko-fi.com/fusedchat' target='_blank'>
  <img height='30' style='border:0;height:38px;' src='https://az743702.vo.msecnd.net/cdn/kofi3.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' />
</a>

# About

KickLib is a C# library that allows for interaction with unofficial / undocumented Kick API (https://kick.com) 
and eases implementation for various bots.

## Features

* Clips
  * Get all clips 
  * Get channel clips
  * Get clip information
* Channels
  * Get channel information
* Livestreams
  * Is streamer live?
  * Get livestream information 
* Users
  * Get user information

## Examples

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

// Get channel clips
var channelClips = await kickApi.Clips.GetChannelClipsAsync(userName);
```

## Custom downloader client

If you are not satisfied with provided client, you can implement your own download logic. 
All you need to do is implement `IApiCaller` interface and pass new instance to `KickApi`.

```csharp
public class MyOwnDownloader : IApiCaller 
{
    // Implementation
}
```
```csharp
var myDownloader = new MyOwnDownloader();
IKickApi kickApi = new KickApi(myDownloader);
```

## Limitations

Currently KickLib is not able to do authenticated calls, because any authentication to Kick's API is very tricky
and they do not provide any official ways to authenticate. Thus library uses only endpoints that don't need this 
(but there are plans to extend it in the future).

# Disclaimer

Kick don't have any official API. All functionality in KickLib was researched and reversed-engineered from their website.
This means that any API can change without announce.

KickLib is meant to be used for education purposes. Don't use it for heavy scraping or other harmful actions against
Kick streaming platform. I don't take responsibility for any KickLib misuse and I strongly advice against such actions.

Once API is officially released, this library will be adjusted accordingly.

# License

See [MIT License](LICENSE)