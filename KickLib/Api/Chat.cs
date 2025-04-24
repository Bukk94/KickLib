using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Chat;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IChat" />
public class Chat : ApiBase, IChat
{
    private const string ApiUrlPart = "chat";

    /// <inheritdoc />
    public Chat(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Chat> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }
    
    /// <summary>
    ///     Post a chat message to a channel as a user.
    ///     When sending as a user, the broadcaster_user_id is required.
    /// </summary>
    public Task<Result<SendChatMessageResponse>> SendMessageAsUserAsync(
        int broadcasterId, 
        string message,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        
        return PostMessageInternalAsync(message, MessageType.User, broadcasterId, null, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Post a chat message to a channel as a bot.
    ///     As a bot, the message will always be sent to the channel attached to your token.
    /// </summary>
    public Task<Result<SendChatMessageResponse>> SendMessageAsBotAsync(
        string message,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);

        return PostMessageInternalAsync(message, MessageType.Bot, null, null, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Post a chat message as reply to a channel as a user.
    ///     When sending as a user, the broadcaster_user_id is required.
    /// </summary>
    public Task<Result<SendChatMessageResponse>> ReplyToMessageAsUserAsync(
        int broadcasterId, 
        string message,
        string messageId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        ArgumentException.ThrowIfNullOrEmpty(messageId);
        
        return PostMessageInternalAsync(message, MessageType.User, broadcasterId, messageId, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Post a chat message as reply to a channel as a bot.
    ///     As a bot, the message will always be sent to the channel attached to your token.
    /// </summary>
    public Task<Result<SendChatMessageResponse>> ReplyToMessageAsBotAsync(
        string message,
        string messageId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(messageId);

        return PostMessageInternalAsync(message, MessageType.Bot, null, messageId, accessToken, cancellationToken);
    }

    private async Task<Result<SendChatMessageResponse>> PostMessageInternalAsync(
        string message,
        MessageType type,
        int? broadcasterId,
        string? messageId,
        string? accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);

        var input = new SendMessageRequest(message, type)
        {
            BroadcasterId = broadcasterId,
            ReplyToMessageId = messageId
        };
        
        // v1/chat
        var result = await PostAsync<SendChatMessageResponse, SendMessageRequest>(ApiUrlPart, ApiVersion.v1, input, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChatWrite}");
        }

        return result;
    }
}