using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Livestream
{
    public class VideoResponse
    {
        public int Id { get; set; }
    
        [JsonProperty(PropertyName = "live_stream_id")]
        public int LivestreamId { get; set; }

        public string Slug { get; set; }
    
        public string Thumb { get; set; }
    
        public string S3 { get; set; }
    
        [JsonProperty(PropertyName = "trading_platform_id")]
        public string TradingPlatformId { get; set; }
    
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }
    
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? UpdatedAt { get; set; }
    
        public Guid Uuid { get; set; }
    
        public int Views { get; set; }
    
        [JsonProperty(PropertyName = "deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}