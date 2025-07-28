using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class RewardRedeemedEvent
{
    [JsonProperty(PropertyName = "reward_title")]
    public string RewardTitle { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }

    [JsonProperty(PropertyName = "channel_id")]
    public int ChannelId { get; set; }

    public string Username { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "user_input")]
    public string? UserInput { get; set; }

    [JsonProperty(PropertyName = "reward_background_color")]
    public string RewardBackgroundColor { get; set; } = string.Empty;
}
