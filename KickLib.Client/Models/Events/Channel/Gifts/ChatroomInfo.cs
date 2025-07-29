using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Channel.Gifts;

public class ChatroomInfo
{
    public int Id { get; set; }

    [JsonProperty(PropertyName = "chatable_type")]
    public string ChatableType { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "channel_id")]
    public int ChannelId { get; set; }

    [JsonProperty(PropertyName = "created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty(PropertyName = "updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty(PropertyName = "chat_mode_old")]
    public string ChatModeOld { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "chat_mode")]
    public string ChatMode { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "slow_mode")]
    public bool SlowMode { get; set; }

    [JsonProperty(PropertyName = "chatable_id")]
    public int ChatableId { get; set; }

    [JsonProperty(PropertyName = "followers_mode")]
    public bool FollowersMode { get; set; }

    [JsonProperty(PropertyName = "subscribers_mode")]
    public bool SubscribersMode { get; set; }

    [JsonProperty(PropertyName = "emotes_mode")]
    public bool EmotesMode { get; set; }

    [JsonProperty(PropertyName = "message_interval")]
    public int MessageInterval { get; set; }

    [JsonProperty(PropertyName = "following_min_duration")]
    public int FollowingMinDuration { get; set; }
}