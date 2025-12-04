using KickLib.Models.v1.Chat;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Chat APIs allow you to use and interact with the chat that is available on the Kick website.
///     You can send a message as a Bot account or your User account.
/// </summary>
public interface IChat
{
    /// <summary>
    ///     Post a chat message to a channel as a user.
    ///     When sending as a user, the broadcaster_user_id is required.
    /// </summary>
    /// <remarks>
    ///     Required scope: chat:write
    /// </remarks>
    /// <param name="broadcasterId">ID of the broadcaster to send the message to.</param>
    /// <param name="message">Message to send. Max length: 500.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<SendChatMessageResponse>> SendMessageAsUserAsync(
        int broadcasterId, 
        string message, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Post a chat message to a channel as a bot.
    ///     As a bot, the message will always be sent to the channel attached to your token.
    /// </summary>
    /// <remarks>
    ///     Required scope: chat:write
    /// </remarks>
    /// <param name="message">Message to send. Max length: 500.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<SendChatMessageResponse>> SendMessageAsBotAsync(
        string message, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Post a chat message as reply to a channel as a user.
    ///     When sending as a user, the broadcaster_user_id is required.
    /// </summary>
    /// <remarks>
    ///     Required scope: chat:write
    /// </remarks>
    /// <param name="broadcasterId">ID of the broadcaster to send the message to.</param>
    /// <param name="message">Message to send as reply. Max length: 500.</param>
    /// <param name="messageId">ID of the message to reply to.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<SendChatMessageResponse>> ReplyToMessageAsUserAsync(
        int broadcasterId, 
        string message, 
        string messageId, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Post a chat message as reply to a channel as a bot.
    ///     As a bot, the message will always be sent to the channel attached to your token.
    /// </summary>
    /// <remarks>
    ///     Required scope: chat:write
    /// </remarks>
    /// <param name="message">Message to send as reply. Max length: 500.</param>
    /// <param name="messageId">ID of the message to reply to.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<SendChatMessageResponse>> ReplyToMessageAsBotAsync(
        string message, 
        string messageId, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a chat message from a channel.
    /// </summary>
    /// <remarks>
    ///     Required scope: moderation:chat_message:manage
    /// </remarks>
    /// <param name="messageId">ID of the message to delete.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<bool>> DeleteMessageAsync(
        string messageId,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}
