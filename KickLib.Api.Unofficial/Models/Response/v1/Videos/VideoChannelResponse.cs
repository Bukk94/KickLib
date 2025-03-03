using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos
{
    public class VideoChannelResponse
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        public string Slug { get; set; }

        [JsonProperty(PropertyName = "is_banned")]
        public bool IsBanned { get; set; }

        [JsonProperty(PropertyName = "playback_url")]
        public string PlaybackUrl { get; set; }

        [JsonProperty(PropertyName = "name_updated_at")]
        public DateTime? NameUpdatedAt { get; set; }
    
        [JsonProperty(PropertyName = "vod_enabled")]
        public bool VodEnabled { get; set; }
    
        [JsonProperty(PropertyName = "subscription_enabled")]
        public bool SubscriptionEnabled { get; set; }
    
        [JsonProperty(PropertyName = "is_affiliate")]
        public bool IsAffiliate { get; set; }

        public int FollowersCount { get; set; }

        [JsonProperty(PropertyName = "can_host")]
        public bool CanHost { get; set; }
    
        public VideoUserResponse User { get; set; }
    
        public VideoVerifiedResponse Verified { get; set; }
    }
}