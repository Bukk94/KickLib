using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Exceptions;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models.Response;
using KickLib.Api.Unofficial.Models.Response.v1.Channels;
using KickLib.Api.Unofficial.Models.Response.v2.Channels;
using KickLib.Api.Unofficial.Models.Response.v2.Channels.Leaderboards;
using KickLib.Api.Unofficial.Models.Response.v2.Channels.Messages;
using KickLib.Api.Unofficial.Models.Response.v2.Channels.Polls;
using KickLib.Api.Unofficial.Models.Response.v2.Channels.Videos;
using KickLib.Api.Unofficial.Models.Response.v2.Clips;
using KickLib.Exceptions;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Api
{
    /// <summary>
    ///     Get information and data about specific channel.
    /// </summary>
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
            var response = await GetAsync<DataWrapper<CountResponse>>(urlPart, ApiVersion.V1Internal).ConfigureAwait(false);
        
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
        ///     Returns chatroom rules.
        /// </summary>
        /// <param name="channelSlug">Channel slug.</param>
        public async Task<ChatroomRulesResponse> GetChatroomRulesAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }
        
            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/chatroom/rules";
        
            var response = await GetAsync<DataWrapper<ChatroomRulesResponse>>(urlPart, ApiVersion.V2).ConfigureAwait(false);
            return response?.Data;
        }
    
        /// <summary>
        ///     Returns channel links.
        /// </summary>
        /// <param name="channelSlug">Channel slug.</param>
        public Task<LinksResponse> GetLinksAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }
        
            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/links";
        
            return GetAsync<LinksResponse>(urlPart, ApiVersion.V1);
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

        /// <summary>
        ///     Page through latest messages in the channel's chat.
        ///     Each page contains up to 25 messages.
        ///     To get live, real-time messages, use <see cref="IKickClient"/> instead.
        /// </summary>
        /// <param name="channelId">Channel ID to get messages from. ID can be found by calling <see cref="GetChannelInfoAsync"/>.</param>
        /// <param name="nextCursor">Cursor value to get more results.</param>
        public async Task<MessagesResponse> GetChannelMessagesAsync(int channelId, string nextCursor = null)
        {
            if (channelId < 0)
            {
                throw new ArgumentException($"Channel ID must be positive value, but was {channelId}.");
            }
        
            var urlPart = $"{ApiUrlPart}{channelId}/messages";
        
            var query = new List<KeyValuePair<string, string>>();
            if (nextCursor is not null)
            {
                // Add cursor (if any)
                query.Add(new("next", nextCursor));
            }
        
            var wrapper = await GetAsync<DataWrapper<MessagesResponse>>(urlPart, ApiVersion.V2, query).ConfigureAwait(false);

            if (wrapper is null ||
                wrapper.Status.Code != 200)
            {
                throw new KickLibException(
                    $"Could not get data. Kick.com error response [{wrapper?.Status?.Code.ToString() ?? "Unknown"}]: {wrapper?.Status?.Message}");
            }

            return wrapper.Data;
        }

        /// <summary>
        ///     Gets active channel poll.
        /// </summary>
        /// <param name="channelSlug">Channel slug.</param>
        public async Task<PollResponse> GetChannelPollAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }

            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/polls";
        
            var response = await GetAsync<DataWrapper<PollResponse>>(urlPart, ApiVersion.V2).ConfigureAwait(false);
            return response?.Data;
        }
    
        /// <summary>
        ///     Gets channel videos.
        /// </summary>
        /// <param name="channelSlug">Channel slug.</param>
        public Task<ICollection<VideoResponse>> GetChannelVideosAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }

            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/videos";
        
            return GetAsync<ICollection<VideoResponse>>(urlPart, ApiVersion.V2);
        }
    
        /// <summary>
        ///     Gets latest video on the channel.
        /// </summary>
        /// <param name="channelSlug">Channel slug.</param>
        public async Task<LatestVideoResponse> GetChannelLatestVideoAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }

            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/videos/latest";
        
            var response = await GetAsync<DataWrapper<LatestVideoResponse>>(urlPart, ApiVersion.V2).ConfigureAwait(false);

            return response?.Data;
        }
    
        /// <summary>
        ///     Gets channel leaderboards.
        /// </summary>
        /// <param name="channelSlug">Channel slug.</param>
        public Task<LeaderboardsResponse> GetChannelLeaderboardsAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }

            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}/leaderboards";
        
            return GetAsync<LeaderboardsResponse>(urlPart, ApiVersion.V2);
        }
    }
}