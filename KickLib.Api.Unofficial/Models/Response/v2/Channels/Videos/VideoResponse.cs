using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Videos
{
    public class VideoResponse
    {
        public long Id { get; set; }

        public string Slug { get; set; }

        [JsonProperty(PropertyName = "channel_id")]
        public long ChannelId { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
    
        [JsonProperty(PropertyName = "session_title")]
        public string SessionTitle { get; set; }
    
        [JsonProperty(PropertyName = "is_live")]
        public bool IsLive { get; set; }
    
        // risk_level_id

        [JsonProperty(PropertyName = "start_time")]
        public DateTime StartTime { get; set; }

        public string Source { get; set; }
    
        [JsonProperty(PropertyName = "twitch_channel")]
        public string TwitchChannel { get; set; }

        public long Duration { get; set; }

        public string Language { get; set; }

        [JsonProperty(PropertyName = "is_mature")]
        public bool IsMature { get; set; }
    
        [JsonProperty(PropertyName = "viewer_count")]
        public int ViewerCount { get; set; }

        public VideoThumbnailResponse Thumbnail { get; set; }

        public int Views { get; set; }
    
        public VideoDetailsResponse Video { get; set; }

        public ICollection<VideoCategoryResponse> Categories { get; set; }
    }
}