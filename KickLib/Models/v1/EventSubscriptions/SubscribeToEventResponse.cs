using Newtonsoft.Json;

namespace KickLib.Models.v1.EventSubscriptions;

/// <summary>
///     Response from the Kick API when subscribing to an event.
/// </summary>
public class SubscribeToEventResponse
{
    /// <summary>
    ///     Has successfully subscribed to the event.
    /// </summary>
    public bool HasSubscribed => !string.IsNullOrWhiteSpace(SubscriptionId) && string.IsNullOrWhiteSpace(Error);
    
    /// <summary>
    ///     The ID of the event subscription.
    ///     In case of error (see <see cref="Error"/>), this field will be null.
    /// </summary>
    [JsonProperty("subscription_id")]
    public string? SubscriptionId { get; set; }
    
    /// <summary>
    ///     Name of the subscribed event.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    ///     Version of subscribed event.
    /// </summary>
    public int Version { get; set; }
    
    /// <summary>
    ///     In case of error, this field contains error details.
    /// </summary>
    public string? Error { get; set; }
}