using Newtonsoft.Json;

namespace KickLib.Models.v1.Channels;

/// <summary>
///     Request to update channel information.
/// </summary>
public class UpdateChannelRequest
{
    /// <summary>
    ///     Category ID to update the channel's category.
    /// </summary>
    [JsonProperty(PropertyName = "category_id")]
    public int? CategoryId { get; set; }
    
    /// <summary>
    ///     New stream title.
    /// </summary>
    [JsonProperty(PropertyName = "stream_title")]
    public string? StreamTitle { get; set; }
}