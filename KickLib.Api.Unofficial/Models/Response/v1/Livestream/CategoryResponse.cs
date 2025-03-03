using KickLib.Api.Unofficial.Models.Response.v1.Channels;
using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Livestream
{
    public class CategoryResponse
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

        public BannerResponse Banner { get; set; }

        public ParentCategoryResponse Category { get; set; }
    }
}