using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Channel.Gifts;

public class GiftsLeaderboard
{
    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public int Quantity { get; set; }
}