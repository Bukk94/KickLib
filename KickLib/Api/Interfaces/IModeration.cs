namespace KickLib.Api.Interfaces;

/// <summary>
///     Interact with moderation endpoints on Kick.com.
/// </summary>
public interface IModeration
{
    /// <summary>
    ///     Permanently ban a user from participating in a broadcaster's chat room.
    /// </summary>
    /// <paramref name="broadcasterUserId">Broadcaster ID in which chat room to ban the user.</paramref>
    /// <paramref name="userIdToBan">User ID to ban.</paramref>
    /// <paramref name="reason">Reason of the ban (Max 100 characters).</paramref>
    Task<Result> BanUserAsync(
        int broadcasterUserId,
        int userIdToBan,
        string? reason = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Timeout a user from participating in a broadcaster's chat room for a specified duration.
    /// </summary>
    /// <paramref name="broadcasterUserId">Broadcaster ID in which chat room to ban the user.</paramref>
    /// <paramref name="userIdToBan">User ID to ban.</paramref>
    /// <paramref name="duration">Timeout duration in minutes (1 - 10080).</paramref>
    /// <paramref name="reason">Reason of the ban (Max 100 characters).</paramref>
    Task<Result> TimeoutUserAsync(
        int broadcasterUserId,
        int userIdToBan,
        int duration,
        string? reason = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Unban or remove timeout that was placed on the specific user in a broadcaster's chat room.
    /// </summary>
    /// <paramref name="broadcasterUserId">Broadcaster ID in which chat room to unban the user.</paramref>
    /// <paramref name="userIdToUnban">User ID to unban.</paramref>
    Task<Result> UnbanUserAsync(
        int broadcasterUserId,
        int userIdToUnban,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}