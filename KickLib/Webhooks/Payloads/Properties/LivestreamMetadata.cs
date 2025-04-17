using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     A Livestream metadata.
/// </summary>
public class LivestreamMetadata
{
    /// <summary>
    ///     Livestream title.
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    ///     Livestream language.
    /// </summary>
    public required string Language { get; set; }
    
    /// <summary>
    ///     Does livestream have mature content?
    /// </summary>
    [JsonProperty(PropertyName = "has_mature_content")]
    public bool HasMatureContent { get; set; }

    /// <summary>
    ///     Livestream category.
    /// </summary>
    public required Category Category { get; set; }
}