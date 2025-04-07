using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Contains user details.
/// </summary>
public class KickUser
{
    /// <summary>
    ///     Is user anonymous?
    ///     If <c>true</c>, all other properties are <c>null</c>!
    /// </summary>
    [JsonProperty(PropertyName = "is_anonymous")]
    public bool IsAnonymous { get; set; }
    
    /// <summary>
    ///     User ID.
    ///     <c>null</c> if <see cref="IsAnonymous"/> is <c>true</c>. Otherwise, value is guaranteed to be non-<c>null</c>.
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public int? UserId { get; set; }
    
    /// <summary>
    ///     User's username.
    ///     <c>null</c> if <see cref="IsAnonymous"/> is <c>true</c>. Otherwise, value is guaranteed to be non-<c>null</c>.
    /// </summary>
    public string? Username { get; set; }
    
    /// <summary>
    ///     Is user verified?
    ///     <c>null</c> if <see cref="IsAnonymous"/> is <c>true</c>. Otherwise, value is guaranteed to be non-<c>null</c>.
    /// </summary>
    [JsonProperty(PropertyName = "is_verified")]
    public bool? IsVerified { get; set; }
    
    /// <summary>
    ///     User's profile picture URL.
    ///     <c>null</c> if <see cref="IsAnonymous"/> is <c>true</c>. Otherwise, value is guaranteed to be non-<c>null</c>.
    /// </summary>
    [JsonProperty(PropertyName = "profile_picture")]
    public string? ProfilePicture { get; set; }
    
    /// <summary>
    ///     User's channel slug (like identifier).
    ///     <c>null</c> if <see cref="IsAnonymous"/> is <c>true</c>. Otherwise, value is guaranteed to be non-<c>null</c>.
    /// </summary>
    [JsonProperty(PropertyName = "channel_slug")]
    public string? ChannelSlug { get; set; }

    /// <summary>
    ///     Identity details like badges.
    /// </summary>
    public UserIdentity? Identity { get; set; }
}