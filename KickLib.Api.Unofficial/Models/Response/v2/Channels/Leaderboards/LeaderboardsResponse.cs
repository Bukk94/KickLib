using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Leaderboards
{
    public class LeaderboardsResponse
    {
        [JsonProperty(PropertyName = "gifts_enabled")]
        public bool GiftsEnabled { get; set; }

        public ICollection<GiftResponse> Gifts { get; set; }
    
        [JsonProperty(PropertyName = "gifts_week_enabled")]
        public bool GiftsWeekEnabled { get; set; }
    
        [JsonProperty(PropertyName = "gifts_week")]
        public ICollection<GiftResponse> GiftsWeek { get; set; }
    
        [JsonProperty(PropertyName = "gifts_month_enabled")]
        public bool GiftsMonthEnabled { get; set; }
    
        [JsonProperty(PropertyName = "gifts_month")]
        public ICollection<GiftResponse> GiftsMonth { get; set; }
    }
}