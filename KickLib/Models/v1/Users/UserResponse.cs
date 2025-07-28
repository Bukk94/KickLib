using Newtonsoft.Json;

namespace KickLib.Models.v1.Users;

/// <summary>
///     Response when getting user information.
/// </summary>
public class UserResponse
{
    /// <summary>
    ///     User's unique identifier.
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }

    /// <summary>
    ///     User's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    ///     User's display name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    ///     User's profile picture URL.
    /// </summary>
    [JsonProperty(PropertyName = "profile_picture")]
    public string ProfilePicture { get; set; } = string.Empty;
}