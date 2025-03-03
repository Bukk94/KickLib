using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Videos
{
    public class VideoDetailsResponse
    {
        public long Id { get; set; }

        public Guid Uuid { get; set; }

        public long Views { get; set; }
    
        [JsonProperty(PropertyName = "live_stream_id")]
        public long LivestreamId { get; set; }

        public string Slug { get; set; }
    
        // public string Thumb { get; set; }
    
        // public string S3 { get; set; }
    
        [JsonProperty(PropertyName = "trading_platform_id")]
        public string TradingPlatformId { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
    
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? UpdatedAt { get; set; }
    
        [JsonProperty(PropertyName = "deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}