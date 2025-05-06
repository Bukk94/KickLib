using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models.Response.v1.Livestream;
using KickLib.Api.Unofficial.Models.Response.v2.Livestream;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Api
{
    /// <summary>
    ///     Get information and data about channel livestream.
    /// </summary>
    public class Livestream : BaseApi
    {
        private const string ApiUrlPart = "channels/";

        public Livestream(IApiCaller client, ILogger logger = null)
            : base(client, logger)
        {
        }
    
        /// <summary>
        ///     Returns bool if channel (streamer) is currently live (broadcasting).
        /// </summary>
        /// <param name="channelSlug">Channel name slug.</param>
        public async Task<bool> IsStreamerLiveAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }

            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}";

            // Version 2 contains much less information which is sufficient for this method
            var data = await GetAsync<LivestreamResponseV2>(urlPart, ApiVersion.V2, "livestream").ConfigureAwait(false);

            return data is not null && data.IsLive;
        }

        /// <summary>
        ///     Returns livestream information of given channel.
        ///     If there is no active livestream, null is returned instead.
        /// </summary>
        /// <param name="channelSlug">Channel name slug.</param>
        public Task<LivestreamResponse> GetLivestreamInfoAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }

            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}";

            // Call v1 for more data
            return GetAsync<LivestreamResponse>(urlPart, ApiVersion.V1, "livestream");
        }
    }
}