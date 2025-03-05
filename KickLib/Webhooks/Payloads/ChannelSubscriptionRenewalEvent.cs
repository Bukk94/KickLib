using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

public class ChannelSubscriptionRenewalEvent : WebhookEventBase
{
    public required KickUser Subscriber { get; set; }

    public int Duration { get; set; }
    
    [JsonProperty(PropertyName = "created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}