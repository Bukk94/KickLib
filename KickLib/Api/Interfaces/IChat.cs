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
    Task<Result<SendChatMessageResponse>> SendMessageAsUserAsync(int broadcasterId, string message, string? accessToken, CancellationToken cancellationToken);

    /// <summary>
    ///     Post a chat message to a channel as a bot.
    ///     As a bot, the message will always be sent to the channel attached to your token.
    /// </summary>
    Task<Result<SendChatMessageResponse>> SendMessageAsBotAsync(string message, string? accessToken, CancellationToken cancellationToken);

    /// <summary>
    ///     Post a chat message as reply to a channel as a user.
    ///     When sending as a user, the broadcaster_user_id is required.
    /// </summary>
    Task<Result<SendChatMessageResponse>> ReplyToMessageAsUserAsync(int broadcasterId, string message, string messageId, string? accessToken, CancellationToken cancellationToken);

    /// <summary>
    ///     Post a chat message as reply to a channel as a bot.
    ///     As a bot, the message will always be sent to the channel attached to your token.
    /// </summary>
    Task<Result<SendChatMessageResponse>> ReplyToMessageAsBotAsync(string message, string messageId, string? accessToken, CancellationToken cancellationToken);
}
