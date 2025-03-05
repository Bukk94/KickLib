using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

public class ChannelNewSubscriptionEvent : WebhookEventBase
{
    public required KickUser Gifter { get; set; }

    public int Duration { get; set; }

    [JsonProperty(PropertyName = "created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}