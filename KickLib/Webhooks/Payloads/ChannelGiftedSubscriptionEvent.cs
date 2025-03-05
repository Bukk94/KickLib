using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

public class ChannelGiftedSubscriptionEvent : WebhookEventBase
{
    public required KickUser Gifter { get; set; }
    
    public required ICollection<KickUser> Giftees { get; set; }

    [JsonProperty(PropertyName = "created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}