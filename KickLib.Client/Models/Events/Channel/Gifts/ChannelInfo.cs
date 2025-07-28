using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Channel.Gifts;

public class ChannelInfo
{
    public int Id { get; set; }

    [JsonProperty(PropertyName = "playback_url")]
    public string PlaybackUrl { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }

    [JsonProperty(PropertyName = "is_banned")]
    public bool IsBanned { get; set; }

    [JsonProperty(PropertyName = "name_updated_at")]
    public DateTime? NameUpdatedAt { get; set; }

    [JsonProperty(PropertyName = "vod_enabled")]
    public bool VodEnabled { get; set; }

    [JsonProperty(PropertyName = "subscription_enabled")]
    public bool SubscriptionEnabled { get; set; }

    [JsonProperty(PropertyName = "is_affiliate")]
    public bool IsAffiliate { get; set; }

    [JsonProperty(PropertyName = "can_host")]
    public bool CanHost { get; set; }

    public ChatroomInfo Chatroom { get; set; } = new();
}