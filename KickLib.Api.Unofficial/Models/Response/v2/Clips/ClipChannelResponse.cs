using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Clips
{
    public class ClipChannelResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Slug { get; set; }

        [JsonProperty(PropertyName = "profile_picture")]
        public string ProfilePicture { get; set; }
    }
}