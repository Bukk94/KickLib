using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Channel.Gifts;

public class GiftsLeaderboardUpdatedEvent
{
    public ChannelInfo Channel { get; set; }
    
    public ICollection<GiftsLeaderboard> Leaderboard { get; set; }
    
    [JsonProperty(PropertyName = "weekly_leaderboard")]
    public ICollection<GiftsLeaderboard> WeeklyLeaderboard { get; set; }
    
    [JsonProperty(PropertyName = "monthly_leaderboard")]
    public ICollection<GiftsLeaderboard> MonthlyLeaderboard { get; set; }
    
    [JsonProperty(PropertyName = "gifter_id")]
    public int GifterId { get; set; }
    
    [JsonProperty(PropertyName = "gifted_quantity")]
    public int GiftedQuantity { get; set; }
    
    [JsonProperty(PropertyName = "gifter_username")]
    public string GifterUsername { get; set; }
}