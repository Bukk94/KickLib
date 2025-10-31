using KickLib.Models.v1.Users;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Retrieve user information.
/// </summary>
public interface IUsers
{
    /// <summary>
    ///     Retrieve user information based on provided user ID.
    /// </summary>
    /// <remarks>
    ///     Required scope: user:read
    /// </remarks>
    /// <param name="userId">User identifier to retrieve information for.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<UserResponse>> GetUserAsync(
        int userId,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Retrieve user information based on provided user IDs.
    ///     If no user IDs are specified, the information for the currently authorised user will be returned by default.
    /// </summary>
    /// <remarks>
    ///     Required scope: user:read
    /// </remarks>
    /// <param name="userIds">Collection of user identifiers to retrieve information for.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<UserResponse>>> GetUsersAsync(
        ICollection<int>? userIds, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve information for the currently authorised user.
    /// </summary>
    /// <remarks>
    ///     Required scope: user:read
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<UserResponse>> GetMeAsync(
        string? accessToken = null, 
        CancellationToken cancellationToken = default);
}
