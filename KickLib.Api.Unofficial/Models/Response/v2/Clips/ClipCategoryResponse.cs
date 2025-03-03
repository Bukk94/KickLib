using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Clips
{
    public class ClipCategoryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Responsive { get; set; }
    
        public string Banner { get; set; }
    
        [JsonProperty(PropertyName = "parent_category")]
        public string ParentCategory { get; set; }
    }
}