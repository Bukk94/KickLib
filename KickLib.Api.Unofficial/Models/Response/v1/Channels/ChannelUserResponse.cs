using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Channels
{
    public class ChannelUserResponse
    {
        public int Id { get; set; }

        public string Username { get; set; }
    
        [JsonProperty(PropertyName = "agreed_to_terms")]
        public bool AgreedToTerms { get; set; }
    
        [JsonProperty(PropertyName = "email_verified_at")]
        public DateTime? EmailVerifiedAt { get; set; }

        public string Bio { get; set; }
    
        public string Country { get; set; }
    
        public string State { get; set; }
    
        public string City { get; set; }

        public string Twitter { get; set; }

        public string Facebook { get; set; }

        public string Instagram { get; set; }

        public string Youtube { get; set; }

        public string Discord { get; set; }

        public string Tiktok { get; set; }

        [JsonProperty(PropertyName = "profile_pic")]
        public string ProfilePicture { get; set; }
    }
}