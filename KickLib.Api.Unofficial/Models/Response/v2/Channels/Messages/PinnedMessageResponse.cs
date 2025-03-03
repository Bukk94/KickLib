using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Messages
{
    public class PinnedMessageResponse
    {
        public PinnedMessageContentResponse Message { get; set; }
    
        [JsonProperty(PropertyName = "finish_at")]
        public DateTime FinishedAt { get; set; }

        public int Duration { get; set; }
    }
}