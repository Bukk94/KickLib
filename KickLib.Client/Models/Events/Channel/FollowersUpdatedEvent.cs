using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Channel;

public class FollowersUpdatedEvent
{
    public int FollowersCount { get; set; }
    
    [JsonProperty(PropertyName = "channel_id")]
    public int ChannelId { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    /// <summary>
    ///     Time in ticks instead of proper date
    /// </summary>
    [JsonProperty(PropertyName = "created_at")]
    public int CreatedAt { get; set; }
    
    public bool Followed { get; set; }
}