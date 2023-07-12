using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response.v2.Clips;

namespace KickLib.Api;

public class Clips : BaseApi
{
    private const string ApiUrlPart = "clips";

    public Clips(IApiCaller client)
        : base(client)
    {
    }
    
    /// <summary>
    ///     List through all clips (across whole platform).
    ///     By default, first 20 entries are returned. To page to more result, use <param name="offset">offset</param> value. 
    /// </summary>
    /// <param name="offset">Offset value to get more results.</param>
    public Task<ClipsResponse> GetClipsAsync(int offset = 0)
    {
        if (offset < 0)
        {
            throw new ArgumentException("Offset must be bigger than 0");
        }

        var query = new List<KeyValuePair<string, string>>
        {
            new("cursor", offset.ToString()),
            new("sort", "view"),
            new("time", "all"),
        };
        
        return GetAsync<ClipsResponse>(ApiUrlPart, ApiVersion.V2, query);
    }
    
    /// <summary>
    ///     Gets clips for specific channel.
    ///     By default, first 20 entries are returned. To page to more result, use <param name="offset">offset</param> value.  
    /// </summary>
    /// <param name="channel">Channel name (slug).</param>
    /// <param name="offset">Offset value to get more results.</param>
    public Task<ClipsResponse> GetChannelClipsAsync(string channel, int offset = 0)
    {
        if (string.IsNullOrWhiteSpace(channel))
        {
            throw new ArgumentNullException(nameof(channel));
        }

        if (offset < 0)
        {
            throw new ArgumentException("Offset must be bigger than 0");
        }

        var urlPart = $"channels/{Uri.EscapeDataString(channel)}/{ApiUrlPart}";
        var query = new List<KeyValuePair<string, string>>
        {
            new("cursor", offset.ToString()),
            new("sort", "view"),
            new("time", "all"),
        };
        
        return GetAsync<ClipsResponse>(urlPart, ApiVersion.V2, query);
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
        var clip = await GetClipAsync(clipId);
        if (clip is null)
        {
            return null;
        }
        
        using var client = new HttpClient();
        var response = await client.GetAsync(clip.VideoUrl);
        response.EnsureSuccessStatusCode();

        using var memoryStream = new MemoryStream();
        await response.Content.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return memoryStream.ToArray();
    }
}