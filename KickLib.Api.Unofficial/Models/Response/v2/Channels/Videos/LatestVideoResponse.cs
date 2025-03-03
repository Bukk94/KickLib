using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Videos
{
    public class LatestVideoResponse
    {
        [JsonProperty(PropertyName = "session_title")]
        public string SessionTitle { get; set; }

        public VideoThumbnailResponse Thumbnail { get; set; }

        public LatestVideoDetailsResponse Video { get; set; }
    }
}