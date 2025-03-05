using Newtonsoft.Json;

namespace KickLib.Models.v1.EventSubscriptions;

public class SubscribeToEventResponse
{
    [JsonProperty("subscription_id")]
    public required string SubscriptionId { get; set; }
    
    public required string Name { get; set; }
    
    public int Version { get; set; }
    
    public string? Error { get; set; }
}