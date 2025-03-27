using Newtonsoft.Json;

namespace KickLib.Models.v1.EventSubscriptions;

/// <summary>
///     Response when subscribing to an event.
/// </summary>
public class EventSubscriptionResponse
{
    /// <summary>
    ///    ID of the subscription.
    /// </summary>
    public required string Id { get; set; }
    
    /// <summary>
    ///     Application ID used for the subscription.
    /// </summary>
    [JsonProperty("app_id")]
    public required string AppId { get; set; }
    
    /// <summary>
    ///     User ID of the broadcaster associated with the subscription.
    /// </summary>
    [JsonProperty("broadcaster_user_id")]
    public int BroadcasterUserId { get; set; }
    
    /// <summary>
    ///     Date and time when the subscription was created.
    /// </summary>
    [JsonProperty("created_at")]
    public required string CreatedAt { get; set; }
    
    /// <summary>
    ///     Event type of the subscription.
    /// </summary>
    public required string Event { get; set; }
    
    /// <summary>
    ///     Subscription method.
    /// </summary>
    public required string Method { get; set; }
    
    /// <summary>
    ///     Date and time when the subscription was last updated.
    /// </summary>
    [JsonProperty("updated_at")]
    public string? UpdatedAt { get; set; }
    
    /// <summary>
    ///     Subscription version.
    /// </summary>
    public int Version { get; set; }
}