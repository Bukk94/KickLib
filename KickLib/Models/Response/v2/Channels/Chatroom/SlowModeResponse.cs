using Newtonsoft.Json;

namespace KickLib.Models.Response.v2.Channels.Chatroom;

public class SlowModeResponse
{
    public bool Enabled { get; set; }

    [JsonProperty(PropertyName = "message_interval")]
    public int MessageInterval { get; set; }
}