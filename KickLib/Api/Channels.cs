using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response.v1.Channels;
using KickLib.Models.Response.v2.Channels;

namespace KickLib.Api;

public class Channels : BaseApi
{
    private const string ApiUrlPart = "channels/";

    public Channels(IApiCaller client)
        : base(client)
    {
    }
    
    /// <summary>
    ///     Gets channel information.
    /// </summary>
    /// <param name="channel">Channel name (slug).</param>
    public Task<ChannelResponse> GetChannelInfoAsync(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channel)}";
        return GetAsync<ChannelResponse>(urlPart, ApiVersion.V1);
    }
    
    /// <summary>
    ///     Gets last subscriber and subscribers count of the channel.
    ///     NOTE that you can retrieve this information only for your own channel!
    /// </summary>
    /// <param name="channel">Channel name (slug).</param>
    public Task<LastSubscriberResponse> GetLastSubscriberAsync(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channel)}/subscribers/last";
        return GetAuthenticatedAsync<LastSubscriberResponse>(urlPart, ApiVersion.V2);
    }
}