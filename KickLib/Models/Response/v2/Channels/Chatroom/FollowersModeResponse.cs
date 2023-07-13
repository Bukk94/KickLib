using Newtonsoft.Json;

namespace KickLib.Models.Response.v2.Channels.Chatroom;

public class FollowersModeResponse
{
    public bool Enabled { get; set; }

    [JsonProperty(PropertyName = "min_duration")]
    public int MinDuration { get; set; }
}