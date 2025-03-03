using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Users
{
    public class UserResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Bio { get; set; }

        public string Twitter { get; set; }

        public string Facebook { get; set; }

        public string Instagram { get; set; }

        public string Youtube { get; set; }

        public string Discord { get; set; }

        public string Tiktok { get; set; }

        [JsonProperty(PropertyName = "profilepic")]
        public string ProfilePicture { get; set; }
    }
}