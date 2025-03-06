using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

public class LivestreamStatusUpdatedEvent : WebhookEventBase
{
    [JsonProperty(PropertyName = "is_live")]
    public bool IsLive { get; set; }
    
    public required string Title { get; set; }
    
    [JsonProperty(PropertyName = "started_at")]
    public required DateTimeOffset StartedAt { get; set; }
    
    [JsonProperty(PropertyName = "ended_at")]
    public required DateTimeOffset? EndedAt { get; set; }
}