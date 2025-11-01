using KickLib.Models.v1.Channels;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Interact with and manipulate channels.
/// </summary>
public interface IChannels
{
    /// <summary>
    ///     Retrieve channel information based on provided broadcaster ID.
    ///     If no broadcaster ID is specified, the information for the currently authorised broadcaster will be returned by default.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:read
    /// </remarks>
    /// <param name="broadcasterUserId">User identifier to retrieve channel information for.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ChannelResponse>> GetChannelAsync(
        int broadcasterUserId, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve channel information based on provided broadcaster IDs.
    ///     If no broadcaster IDs are specified, the information for the currently authorised broadcaster will be returned by default.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:read
    /// </remarks>
    /// <param name="broadcasterUserIds">User identifiers to retrieve channel information for.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<ChannelResponse>>> GetChannelsAsync(
        ICollection<int> broadcasterUserIds, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve channel information based on provided broadcaster slug (unique username).
    ///     If no slug is specified, the information for the currently authorised broadcaster will be returned by default.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:read
    /// </remarks>
    /// <param name="slug">User's slug (unique username).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ChannelResponse>> GetChannelAsync(
        string slug, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve channel information based on provided broadcaster slugs (unique username).
    ///     If no slugs are specified, the information for the currently authorised broadcaster will be returned by default.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:read
    /// </remarks>
    /// <param name="slugs">User's slugs (unique usernames).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<ChannelResponse>>> GetChannelsAsync(
        ICollection<string> slugs, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve channel information for the currently authorised user.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:read
    ///     Optional scope: streamkey:read (if you want to retrieve the stream key information)
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ChannelResponse>> GetMyChannelAsync(
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Update channel information.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:write
    /// </remarks>
    /// <param name="request">Request object containing the channel update information.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<bool>> UpdateChannelAsync(
        UpdateChannelRequest request, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);
}
