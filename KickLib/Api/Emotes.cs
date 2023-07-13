using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response.v1.Emotes;

namespace KickLib.Api;

public class Emotes : BaseApi
{
    private const string ApiUrlPart = "emotes/";

    public Emotes(IApiCaller client)
        : base(client)
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