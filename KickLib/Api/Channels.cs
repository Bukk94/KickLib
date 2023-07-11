using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response.v1.Channels;

namespace KickLib.Api;

public class Channels : BaseApi
{
    private const string ApiUrlPart = "channels/";

    public Channels(IApiCaller client)
        : base(client)
    {
    }
    
    public Task<ChannelResponse> GetChannelInfoAsync(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(channel)}";
        return GetAsync<ChannelResponse>(urlPart, ApiVersion.V1);
    }
}