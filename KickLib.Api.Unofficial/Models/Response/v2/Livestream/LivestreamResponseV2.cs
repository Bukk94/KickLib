using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Livestream
{
    public class LivestreamResponseV2
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

        // TODO: source

        [JsonProperty(PropertyName = "twitch_channel")]
        public string TwitchChannel { get; set; }

        public int Duration { get; set; }

        public string Language { get; set; }

        [JsonProperty(PropertyName = "is_mature")]
        public bool IsMature { get; set; }

        [JsonProperty(PropertyName = "viewer_count")]
        public int ViewerCount { get; set; }

        public ThumbnailResponseV2 Thumbnail { get; set; }

        public int Viewers { get; set; }

        public ICollection<CategoryResponseV2> Categories { get; set; }

        public ICollection<string> Tags { get; set; }
    }
}