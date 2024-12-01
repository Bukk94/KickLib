using Newtonsoft.Json;

namespace KickLib.Models.Response.v2.Channels.Leaderboards;

public class GiftResponse
{
    [JsonProperty(PropertyName = "user_id")]
    public string UserId { get; set; }

    public string Username { get; set; }
    
    public int Quantity { get; set; }
}