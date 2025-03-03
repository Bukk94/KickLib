using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Videos
{
    public class VideoCategoryResponse
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "category_id")]
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public ICollection<string> Tags { get; set; }

        public string Description { get; set; }

        [JsonProperty(PropertyName = "deleted_at")]
        public DateTime? DeletedAt { get; set; }

        public int Viewers { get; set; }

        [JsonProperty(PropertyName = "is_mature")]
        public bool IsMature { get; set; }
    
        public VideoBannerResponse Banner { get; set; }
    }
}