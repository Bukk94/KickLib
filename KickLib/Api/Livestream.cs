using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response.v1.Livestream;
using KickLib.Models.Response.v2.Livestream;

namespace KickLib.Api;

public class Livestream : BaseApi
{
    private const string ApiUrlPart = "channels/";

    public Livestream(IApiCaller client)
        : base(client)
    {
    }
    
    /// <summary>
    ///     Returns bool if channel (streamer) is currently live (broadcasting).
    /// </summary>
    /// <param name="channel">Channel name (slug).</param>
    public async Task<bool> IsStreamerLiveAsync(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channel)}";

        // Version 2 contains much less information which is sufficient for this method
        var data = await GetAsync<LivestreamResponseV2>(urlPart, ApiVersion.V2, "livestream");

        return data is not null && data.IsLive;
    }

    /// <summary>
    ///     Returns livestream information of given channel.
    ///     If there is no active livestream, null is returned instead.
    /// </summary>
    /// <param name="channel">Channel name (slug).</param>
    public Task<LivestreamResponse> GetLivestreamInfoAsync(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channel)}";

        // Call v1 for more data
        return GetAsync<LivestreamResponse>(urlPart, ApiVersion.V1, "livestream");
    }
}