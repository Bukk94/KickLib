using Newtonsoft.Json;

namespace KickLib.Models.v1.Channels;

internal class UpdateChannelApiRequest
{
    [JsonProperty(PropertyName = "category_id")]
    public int? CategoryId { get; set; }
    
    [JsonProperty(PropertyName = "stream_title")]
    public string? StreamTitle { get; set; }
    
    [JsonProperty(PropertyName = "custom_tags")]
    public ICollection<string>? CustomTags { get; set; }
    
    internal static UpdateChannelApiRequest FromRequest(UpdateChannelRequest request)
    {
        return new UpdateChannelApiRequest
        {
            CategoryId = request.CategoryId,
            StreamTitle = request.StreamTitle,
            CustomTags = request.CustomTags
        };
    }
}