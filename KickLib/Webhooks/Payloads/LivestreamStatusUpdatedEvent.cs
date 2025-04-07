using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a livestream status updated event.
/// </summary>
public class LivestreamStatusUpdatedEvent : WebhookEventBase
{
    /// <summary>
    ///     Is the stream live?
    /// </summary>
    [JsonProperty(PropertyName = "is_live")]
    public bool IsLive { get; set; }
    
    /// <summary>
    ///    Title of the stream. 
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    ///     When the stream started.
    /// </summary>
    [JsonProperty(PropertyName = "started_at")]
    public required DateTimeOffset StartedAt { get; set; }
    
    /// <summary>
    ///     When the stream ended (if offline).
    /// </summary>
    [JsonProperty(PropertyName = "ended_at")]
    public required DateTimeOffset? EndedAt { get; set; }
}