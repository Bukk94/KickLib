using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models.Response.v2.Clips;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Api
{
    /// <summary>
    ///     Get clips data.
    /// </summary>
    public class Clips : BaseApi
    {
        private const string ApiUrlPart = "clips";

        public Clips(IApiCaller client, ILogger logger = null)
            : base(client, logger)
        {
        }
    
        /// <summary>
        ///     List through all clips (across whole platform).
        ///     By default, first 20 entries are returned. To page to more result, use <param name="nextCursor">cursor</param> value. 
        /// </summary>
        /// <param name="nextCursor">Cursor value to get more results.</param>
        public Task<ClipsResponse> GetClipsAsync(string nextCursor = null)
        {
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
        
            return GetAsync<ClipsResponse>(ApiUrlPart, ApiVersion.V2, query);
        }
    
        /// <summary>
        ///     Gets specific clip by <param name="clipId">Clip ID</param>.
        /// </summary>
        /// <param name="clipId">Id of the clip.</param>
        public Task<ClipResponse> GetClipAsync(int clipId)
        {
            var urlPart = $"{ApiUrlPart}/{clipId}";
            return GetAsync<ClipResponse>(urlPart, ApiVersion.V2, "clip");
        }
    
        public async Task<byte[]> DownloadClipAsync(int clipId)
        {
            var clip = await GetClipAsync(clipId).ConfigureAwait(false);
            if (clip is null)
            {
                return null;
            }
        
            using var client = new HttpClient();
            var response = await client.GetAsync(clip.VideoUrl).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            using var memoryStream = new MemoryStream();
            await response.Content.CopyToAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream.ToArray();
        }
    }
}