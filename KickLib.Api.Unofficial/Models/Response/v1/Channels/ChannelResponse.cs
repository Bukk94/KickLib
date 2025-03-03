using KickLib.Api.Unofficial.Models.Response.v1.Livestream;
using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Channels
{
    public class ChannelResponse
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        public string Slug { get; set; }

        [JsonProperty(PropertyName = "is_banned")]
        public bool IsBanned { get; set; }

        [JsonProperty(PropertyName = "playback_url")]
        public string PlaybackUrl { get; set; }

        [JsonProperty(PropertyName = "vod_enabled")]
        public bool VodEnabled { get; set; }

        [JsonProperty(PropertyName = "subscription_enabled")]
        public bool SubscriptionEnabled { get; set; }

        [JsonProperty(PropertyName = "followersCount")]
        public int FollowersCount { get; set; }

        [JsonProperty(PropertyName = "subscriber_badges")]
        public ICollection<SubscriberBadgeResponse> Type { get; set; }

        [JsonProperty(PropertyName = "banner_image")]
        public BannerResponse BannerImage { get; set; }

        public LivestreamResponse Livestream { get; set; }

        // TODO: role

        public bool Muted { get; set; }
    
        // TODO: follower_badges
    
        [JsonProperty(PropertyName = "offline_banner_image")]
        public OfflineBannerImageResponse OfflineBannerImage { get; set; }

        [JsonProperty(PropertyName = "recent_categories")]
        public ICollection<CategoryResponse> RecentCategories { get; set; }

        [JsonProperty(PropertyName = "can_host")]
        public bool CanHost { get; set; }

        public ChannelUserResponse User { get; set; }

        [JsonProperty(PropertyName = "previous_livestreams")]
        public ICollection<PreviousLivestreamResponse> PreviousLivestreams { get; set; }

        public ChatroomResponse Chatroom { get; set; }
    
        [JsonProperty(PropertyName = "ascending_links")]
        public ICollection<LinkResponse> AscendingLinks { get; set; }
    
        public PlanResponse Plan { get; set; }
    
        public ICollection<MediaResponse> Media { get; set; }
    
        public VerifiedResponse Verified { get; set; }
    }
}