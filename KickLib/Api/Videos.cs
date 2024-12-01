using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response.v1.Videos;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

public class Videos : BaseApi
{
    private const string ApiUrlPart = "video/";

    public Videos(IApiCaller client, ILogger logger = null)
        : base(client, logger)
    {
    }
    
    /// <summary>
    ///     Gets specific video details.
    /// </summary>
    /// <param name="videoUid">Video unique identifier (UUID).</param>
    public Task<VideoResponse> GetVideoAsync(Guid videoUid)
    {
        if (videoUid == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(videoUid));
        }

        var urlPart = $"{ApiUrlPart}{videoUid}";
        return GetAsync<VideoResponse>(urlPart, ApiVersion.V1);
    }
}