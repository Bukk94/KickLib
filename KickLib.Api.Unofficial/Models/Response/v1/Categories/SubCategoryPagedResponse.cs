using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Categories
{
    public class SubCategoryPagedResponse
    {
        public ICollection<SubCategoryResponse> Data { get; set; }

        [JsonProperty(PropertyName = "current_page")]
        public int CurrentPage { get; set; }
    
        [JsonProperty(PropertyName = "first_page_url")]
        public string FirstPageUrl { get; set; }

        public int From { get; set; }
    
        [JsonProperty(PropertyName = "last_page")]
        public int LastPage { get; set; }
    
        [JsonProperty(PropertyName = "last_page_url")]
        public string LastPageUrl { get; set; }
    
        [JsonProperty(PropertyName = "next_page_url")]
        public string NextPageUrl { get; set; }

        public string Path { get; set; }
    
        [JsonProperty(PropertyName = "per_page")]
        public string PerPage { get; set; }
    
        [JsonProperty(PropertyName = "prev_page_url")]
        public string PreviousPageUrl { get; set; }

        public int To { get; set; }
    
        public int Total { get; set; }

        public ICollection<CategoryLinkResponse> Links { get; set; }
    }
}