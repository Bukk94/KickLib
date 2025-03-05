using Newtonsoft.Json;

namespace KickLib.Models.v1.EventSubscriptions;

public class EventSubscriptionResponse
{
    public required string Id { get; set; }
    
    [JsonProperty("app_id")]
    public required string AppId { get; set; }
    
    [JsonProperty("broadcaster_user_id")]
    public int BroadcasterUserId { get; set; }
    
    [JsonProperty("created_at")]
    public required string CreatedAt { get; set; }
    
    public required string Event { get; set; }
    
    public required string Method { get; set; }
    
    [JsonProperty("updated_at")]
    public string? UpdatedAt { get; set; }
    
    public int Version { get; set; }
}