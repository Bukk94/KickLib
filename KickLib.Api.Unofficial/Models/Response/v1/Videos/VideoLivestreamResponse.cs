using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Videos
{
    public class VideoLivestreamResponse
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        [JsonProperty(PropertyName = "channel_id")]
        public int ChannelId { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "session_title")]
        public string SessionTitle { get; set; }

        [JsonProperty(PropertyName = "is_live")]
        public bool IsLive { get; set; }

        [JsonProperty(PropertyName = "risk_level_id")]
        public int? RiskLevelId { get; set; }
    
        [JsonProperty(PropertyName = "start_time")]
        public DateTime? StartTime { get; set; }
    
        // TODO: source

        [JsonProperty(PropertyName = "twitch_channel")]
        public string TwitchChannel { get; set; }

        public int Duration { get; set; }

        public string Language { get; set; }

        [JsonProperty(PropertyName = "is_mature")]
        public bool IsMature { get; set; }

        [JsonProperty(PropertyName = "viewer_count")]
        public int ViewerCount { get; set; }

        public string Thumbnail { get; set; }

        public VideoChannelResponse Channel { get; set; }
    
        public ICollection<VideoCategoryResponse> Categories { get; set; }
    }
}