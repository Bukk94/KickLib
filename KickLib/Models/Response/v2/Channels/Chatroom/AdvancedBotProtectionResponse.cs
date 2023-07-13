using Newtonsoft.Json;

namespace KickLib.Models.Response.v2.Channels.Chatroom;

public class AdvancedBotProtectionResponse
{
    public bool Enabled { get; set; }

    [JsonProperty(PropertyName = "remaining_time")]
    public int RemainingTime { get; set; }
}