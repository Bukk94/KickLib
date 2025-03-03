using Newtonsoft.Json;

namespace KickLib.Models.v1.Users;

public class UserResponse
{
    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }

    public required string Email { get; set; }
    
    public required string Name { get; set; }
    
    [JsonProperty(PropertyName = "profile_picture")]
    public required string ProfilePicture { get; set; }
}