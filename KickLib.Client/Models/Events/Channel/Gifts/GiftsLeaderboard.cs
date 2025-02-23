using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Channel.Gifts;

public class GiftsLeaderboard
{
    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }

    public required string Username { get; set; }

    public int Quantity { get; set; }
}