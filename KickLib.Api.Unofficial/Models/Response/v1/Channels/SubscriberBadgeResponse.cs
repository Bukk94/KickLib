using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Channels
{
    public class SubscriberBadgeResponse
    {
        public int Id { get; set; }
    
        [JsonProperty(PropertyName = "channel_id")]
        public int ChannelId { get; set; }
    
        public int Months { get; set; }
  
        [JsonProperty(PropertyName = "badge_image")]
        public BadgeImageResponse BadgeImage { get; set; }
    }
}