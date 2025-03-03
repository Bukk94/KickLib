using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Clips
{
    public class ClipResponse
    {
        public string Id { get; set; }
    
        [JsonProperty(PropertyName = "is_mature")]
        public bool IsMature { get; set; }
    
        public string Title { get; set; }
    
        public int Duration { get; set; }
    
        [JsonProperty(PropertyName = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }
    
        [JsonProperty(PropertyName = "video_url")]
        public string VideoUrl { get; set; }
    
        [JsonProperty(PropertyName = "view_count")]
        public int ViewCount { get; set; }
    
        [JsonProperty(PropertyName = "likes_count")]
        public int LikesCount { get; set; }
    
        public bool Liked { get; set; }
    
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        public CreatorResponse Creator { get; set; }
    
        public ClipChannelResponse Channel { get; set; }
    
        public ClipCategoryResponse Category { get; set; }
    }
}
