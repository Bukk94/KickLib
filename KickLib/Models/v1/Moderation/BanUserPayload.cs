using Newtonsoft.Json;

namespace KickLib.Models.v1.Moderation;

internal class BanUserPayload
{
    [JsonProperty(PropertyName = "broadcaster_user_id")]
    public int BroadcasterId { get; set; }
    
    [JsonProperty(PropertyName = "user_id")]
    public int UserIdToBan { get; set; }

    public string? Reason { get; set; }

    public int? Duration { get; set; }
}