using KickLib.Api.Unofficial.Models.Response.v2.Clips;
using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Categories
{
    public class CategoryClipResponse
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "livestream_id")]
        public string LivestreamId { get; set; }
    
        [JsonProperty(PropertyName = "category_id")]
        public string CategoryId { get; set; }
    
        [JsonProperty(PropertyName = "channel_id")]
        public int ChannelId { get; set; }
    
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }
    
        public string Title { get; set; }
    
        [JsonProperty(PropertyName = "is_mature")]
        public bool IsMature { get; set; }
    
        [JsonProperty(PropertyName = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty(PropertyName = "clip_url")]
        public string ClipUrl { get; set; }
    
        [JsonProperty(PropertyName = "video_url")]
        public string VideoUrl { get; set; }

        public int Duration { get; set; }

        public string Privacy { get; set; }
    
        [JsonProperty(PropertyName = "view_count")]
        public int ViewCount { get; set; }

        [JsonProperty(PropertyName = "likes_count")]
        public int LikesCount { get; set; }

        public bool Liked { get; set; }
    
        public int Likes { get; set; }

        [JsonProperty(PropertyName = "started_at")]
        public DateTime StartedAt { get; set; }
    
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        public CreatorResponse Creator { get; set; }

        public ClipChannelResponse Channel { get; set; }

        public ClipCategoryResponse Category { get; set; }
    }
}