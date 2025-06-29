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
    Task<Result<UserResponse>> GetUserAsync(
        int userId,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Retrieve user information based on provided user IDs.
    ///     If no user IDs are specified, the information for the currently authorised user will be returned by default.
    /// </summary>
    Task<Result<ICollection<UserResponse>>> GetUsersAsync(ICollection<int>? userIds, string? accessToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve information for the currently authorised user.
    /// </summary>
    Task<Result<UserResponse>> GetMeAsync(string? accessToken = null, CancellationToken cancellationToken = default);
}
