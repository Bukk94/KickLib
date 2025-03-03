using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models.Response.v1.Emotes;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Api
{
    /// <summary>
    ///     Get emotes data.
    /// </summary>
    public class Emotes : BaseApi
    {
        private const string ApiUrlPart = "emotes/";

        public Emotes(IApiCaller client, ILogger logger = null)
            : base(client, logger)
        {
        }
    
        /// <summary>
        ///     Returns channel emotes.
        /// </summary>
        /// <param name="channelSlug">Channel slug.</param>
        public Task<EmotesListResponse> GetChannelEmotesAsync(string channelSlug)
        {
            if (string.IsNullOrWhiteSpace(channelSlug))
            {
                throw new ArgumentNullException(nameof(channelSlug));
            }
        
            var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channelSlug)}";
        
            return GetAsync<EmotesListResponse>(urlPart, ApiVersion.None);
        }
    }
}