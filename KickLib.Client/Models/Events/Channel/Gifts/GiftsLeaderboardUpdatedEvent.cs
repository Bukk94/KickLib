using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Channel.Gifts;

public class GiftsLeaderboardUpdatedEvent
{
    public required ChannelInfo Channel { get; set; }

    public ICollection<GiftsLeaderboard> Leaderboard { get; set; } = new List<GiftsLeaderboard>();
    
    [JsonProperty(PropertyName = "weekly_leaderboard")]
    public ICollection<GiftsLeaderboard> WeeklyLeaderboard { get; set; } = new List<GiftsLeaderboard>();
    
    [JsonProperty(PropertyName = "monthly_leaderboard")]
    public ICollection<GiftsLeaderboard> MonthlyLeaderboard { get; set; } = new List<GiftsLeaderboard>();
    
    [JsonProperty(PropertyName = "gifter_id")]
    public int GifterId { get; set; }
    
    [JsonProperty(PropertyName = "gifted_quantity")]
    public int GiftedQuantity { get; set; }
    
    [JsonProperty(PropertyName = "gifter_username")]
    public required string GifterUsername { get; set; }
}