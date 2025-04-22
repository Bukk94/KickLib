using KickLib.Models.v1.Channels;

namespace KickLib.Api.Interfaces;

/// <summary>
/// Interact with and manipulate channels.
/// </summary>
public interface IChannels
{
    /// <summary>
    ///     Retrieve channel information based on provided streamer ID.
    ///     If no streamer ID is specified, the information for the currently authorised streamer will be returned by default.
    /// </summary>
    Task<Result<ChannelResponse>> GetChannelAsync(int broadcasterUserId, string? accessToken, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieve channel information based on provided streamer IDs.
    ///     If no streamer IDs are specified, the information for the currently authorised streamer will be returned by default.
    /// </summary>
    Task<Result<ICollection<ChannelResponse>>> GetChannelsAsync(ICollection<int> broadcasterUserIds, string? accessToken, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieve channel information based on provided streamer slug (unique username).
    ///     If no slug is specified, the information for the currently authorised streamer will be returned by default.
    /// </summary>
    Task<Result<ChannelResponse>> GetChannelAsync(string slug, string? accessToken, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieve channel information based on provided streamer slugs (unique username).
    ///     If no slugs are specified, the information for the currently authorised streamer will be returned by default.
    /// </summary>
    Task<Result<ICollection<ChannelResponse>>> GetChannelsAsync(ICollection<string> slugs, string? accessToken, CancellationToken cancellationToken);

    /// <summary>
    ///     Retrieve channel information for the currently authorised user.
    /// </summary>
    Task<Result<ChannelResponse>> GetMyChannelAsync(string? accessToken, CancellationToken cancellationToken);

    /// <summary>
    ///     Update channel information.
    /// </summary>
    Task<Result<bool>> UpdateChannelAsync(UpdateChannelRequest request, string? accessToken, CancellationToken cancellationToken = default);
}
