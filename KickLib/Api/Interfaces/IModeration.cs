using KickLib.Models.v1.Moderation;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Interact with moderation endpoints on Kick.com.
/// </summary>
public interface IModeration
{
    /// <summary>
    ///     Permanently ban a user from participating in a broadcaster's chat room.
    /// </summary>
    /// <remarks>
    ///     Required scope: moderation:ban
    /// </remarks>
    /// <param name="broadcasterUserId">Broadcaster ID in which chat room to ban the user.</param>
    /// <param name="userIdToBan">User ID to ban.</param>
    /// <param name="reason">Reason of the ban (Max 100 characters).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result> BanUserAsync(
        int broadcasterUserId,
        int userIdToBan,
        string? reason = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Timeout a user from participating in a broadcaster's chat room for a specified duration.
    /// </summary>
    /// <remarks>
    ///     Required scope: moderation:ban
    /// </remarks>
    /// <param name="broadcasterUserId">Broadcaster ID in which chat room to ban the user.</param>
    /// <param name="userIdToBan">User ID to ban.</param>
    /// <param name="duration">Timeout duration in minutes (1 - 10080).</param>
    /// <param name="reason">Reason of the ban (Max 100 characters).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result> TimeoutUserAsync(
        int broadcasterUserId,
        int userIdToBan,
        int duration,
        string? reason = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Timeout a user from participating in a broadcaster's chat room for a specified duration.
    /// </summary>
    /// <remarks>
    ///     Required scope: moderation:ban
    /// </remarks>
    /// <param name="broadcasterUserId">Broadcaster ID in which chat room to ban the user.</param>
    /// <param name="userIdToBan">User ID to ban.</param>
    /// <param name="duration">Timeout duration with built-in validation.</param>
    /// <param name="reason">Reason of the ban (Max 100 characters).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result> TimeoutUserAsync(
        int broadcasterUserId,
        int userIdToBan,
        TimeoutDuration duration,
        string? reason = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Unban or remove timeout that was placed on the specific user in a broadcaster's chat room.
    /// </summary>
    /// <remarks>
    ///     Required scope: moderation:ban
    /// </remarks>
    /// <param name="broadcasterUserId">Broadcaster ID in which chat room to unban the user.</param>
    /// <param name="userIdToUnban">User ID to unban.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result> UnbanUserAsync(
        int broadcasterUserId,
        int userIdToUnban,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}