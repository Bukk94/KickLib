using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response;
using KickLib.Models.Response.v1.Channels;
using KickLib.Models.Response.v2.Channels;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

public class Channels : BaseApi
{
    private const string ApiUrlPart = "channels/";

    public Channels(IApiCaller client, ILogger logger = null)
        : base(client, logger)
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
    
    /// <summary>
    ///     Gets follower count of the channel.
    /// </summary>
    /// <param name="channel">Channel name (slug).</param>
    public async Task<int?> GetFollowersCountAsync(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channel)}/followers-count";
        var response = await GetAsync<DataWrapper<CountResponse>>(urlPart, ApiVersion.V1Internal);
        
        return response?.Data?.Count;
    }
    
    /// <summary>
    ///     Returns chatroom information of given channel.
    /// </summary>
    /// <param name="channel">Channel name (slug).</param>
    public Task<ChatroomResponseV2> GetChatroomAsync(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }
        
        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channel)}/chatroom";
        
        return GetAsync<ChatroomResponseV2>(urlPart, ApiVersion.V2);
    }
}