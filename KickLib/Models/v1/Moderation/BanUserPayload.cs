using Newtonsoft.Json;

namespace KickLib.Models.v1.Moderation;

internal class BanUserPayload
{
    [JsonProperty(PropertyName = "broadcaster_user_id")]
    public required int BroadcasterId { get; init; }
    
    [JsonProperty(PropertyName = "user_id")]
    public required int UserIdToBan { get; init; }

    public string? Reason { get; set; }

    public int? Duration { get; set; }
}