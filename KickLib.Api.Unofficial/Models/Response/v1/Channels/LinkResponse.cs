using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Channels
{
    public class LinkResponse
    {
        public int Id { get; set; }
    
        [JsonProperty(PropertyName = "channel_id")]
        public int ChannelId { get; set; }
    
        public string Description { get; set; }
    
        public string Link { get; set; }
    
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }
    
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public int Order { get; set; }
    
        public string Title { get; set; }
    }
}