using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

public class KickUser
{
    [JsonProperty(PropertyName = "is_anonymous")]
    public bool IsAnonymous { get; set; }
    
    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }
    
    public required string Username { get; set; }
    
    [JsonProperty(PropertyName = "is_verified")]
    public bool IsVerified { get; set; }
    
    [JsonProperty(PropertyName = "profile_picture")]
    public required string ProfilePicture { get; set; }
    
    [JsonProperty(PropertyName = "channel_slug")]
    public required string ChannelSlug { get; set; }
}