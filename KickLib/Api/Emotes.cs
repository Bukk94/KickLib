using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response.v1.Emotes;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

public class Emotes : BaseApi
{
    private const string ApiUrlPart = "emotes/";

    public Emotes(IApiCaller client, ILogger logger = null)
        : base(client, logger)
    {
    }
    
    public Task<EmotesListResponse> GetChannelEmotesAsync(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }
        
        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channel)}";
        
        return GetAsync<EmotesListResponse>(urlPart, ApiVersion.None);
    }
}