using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels
{
    public class LastSubscriberResponse
    {
        [JsonProperty(PropertyName = "last_subscriber")]
        public string LastSubscriber { get; set; }
    
        public int Count { get; set; }
    }
}