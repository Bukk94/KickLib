using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response;
using KickLib.Models.Response.v1.Channels;
using KickLib.Models.Response.v2.Channels;
using KickLib.Models.Response.v2.Clips;
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
    /// <param name="channelSlug">Channel slug.</param>
    public Task<ChannelResponse> GetChannelInfoAsync(string channelSlug)
    {
        if (string.IsNullOrWhiteSpace(channelSlug))
        {
            throw new ArgumentNullException(nameof(channelSlug));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}";
        return GetAsync<ChannelResponse>(urlPart, ApiVersion.V1);
    }
    
    /// <summary>
    ///     [Auth Required] Gets last subscriber and subscribers count of the channel.
    ///     NOTE that you can retrieve this information only for your own channel!
    /// </summary>
    /// <param name="channelSlug">Channel slug.</param>
    public Task<LastSubscriberResponse> GetLastSubscriberAsync(string channelSlug)
    {
        if (string.IsNullOrWhiteSpace(channelSlug))
        {
            throw new ArgumentNullException(nameof(channelSlug));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/subscribers/last";
        return GetAuthenticatedAsync<LastSubscriberResponse>(urlPart, ApiVersion.V2);
    }
    
    /// <summary>
    ///     Gets follower count of the channel.
    /// </summary>
    /// <param name="channelSlug">Channel slug.</param>
    public async Task<int?> GetFollowersCountAsync(string channelSlug)
    {
        if (string.IsNullOrWhiteSpace(channelSlug))
        {
            throw new ArgumentNullException(nameof(channelSlug));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/followers-count";
        var response = await GetAsync<DataWrapper<CountResponse>>(urlPart, ApiVersion.V1Internal);
        
        return response?.Data?.Count;
    }
    
    /// <summary>
    ///     Returns chatroom information of given channel.
    /// </summary>
    /// <param name="channelSlug">Channel slug.</param>
    public Task<ChatroomResponseV2> GetChatroomAsync(string channelSlug)
    {
        if (string.IsNullOrWhiteSpace(channelSlug))
        {
            throw new ArgumentNullException(nameof(channelSlug));
        }
        
        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/chatroom";
        
        return GetAsync<ChatroomResponseV2>(urlPart, ApiVersion.V2);
    }
    
    /// <summary>
    ///     Gets clips for specific channel.
    ///     By default, first 20 entries are returned. To page to more result, use <param name="nextCursor">cursor</param> value.  
    /// </summary>
    /// <param name="channelSlug">Channel slug.</param>
    /// <param name="nextCursor">Cursor value to get more results.</param>
    public Task<ClipsResponse> GetChannelClipsAsync(string channelSlug, string nextCursor = null)
    {
        if (string.IsNullOrWhiteSpace(channelSlug))
        {
            throw new ArgumentNullException(nameof(channelSlug));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/clips";
        var query = new List<KeyValuePair<string, string>>
        {
            new("sort", "view"),
            new("time", "all"),
        };

        if (nextCursor is not null)
        {
            // Add cursor (if any)
            query.Add(new("cursor", nextCursor));
        }
        
        return GetAsync<ClipsResponse>(urlPart, ApiVersion.V2, query);
    }
}