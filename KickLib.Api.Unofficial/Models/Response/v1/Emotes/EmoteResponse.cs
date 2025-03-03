using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Emotes
{
    public class EmoteResponse
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "channel_id")]
        public int? ChannelId { get; set; }

        public string Name { get; set; }
    
        [JsonProperty(PropertyName = "subscribers_only")]
        public bool SubscribersOnly { get; set; }
    }
}