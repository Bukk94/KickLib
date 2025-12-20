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
    
    /// <inheritdoc />
    public Task<Result<SendChatMessageResponse>> SendMessageAsUserAsync(
        int broadcasterId, 
        string message,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        }
        
        return PostMessageInternalAsync(message, MessageType.User, broadcasterId, null, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<Result<SendChatMessageResponse>> SendMessageAsBotAsync(
        string message,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        }
        
        return PostMessageInternalAsync(message, MessageType.Bot, null, null, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<Result<SendChatMessageResponse>> ReplyToMessageAsUserAsync(
        int broadcasterId, 
        string message,
        string messageId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        }
        if (string.IsNullOrEmpty(messageId))
        {
            throw new ArgumentException("Message ID cannot be null or empty.", nameof(messageId));
        }

        return PostMessageInternalAsync(message, MessageType.User, broadcasterId, messageId, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<Result<SendChatMessageResponse>> ReplyToMessageAsBotAsync(
        string message,
        string messageId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(messageId))
        {
            throw new ArgumentException("Message ID cannot be null or empty.", nameof(messageId));
        }

        return PostMessageInternalAsync(message, MessageType.Bot, null, messageId, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<bool>> DeleteMessageAsync(
        string messageId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(messageId))
        {
            throw new ArgumentException("MessageId cannot be null or empty.", nameof(messageId));
        }
        
        // v1/chat/{id}
        var urlPart = $"{ApiUrlPart}/{messageId}";
        
        var result = await DeleteAsync(urlPart, ApiVersion.v1, accessToken: accessToken, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ModerationChatMessageManage}");
        }
        
        return result;
    }

    private async Task<Result<SendChatMessageResponse>> PostMessageInternalAsync(
        string message,
        MessageType type,
        int? broadcasterId,
        string? messageId,
        string? accessToken,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(message))
        {
            throw new ArgumentException("Message cannot be null or empty.", nameof(message));
        }

        var input = new SendMessageApiRequest(message, type)
        {
            BroadcasterId = broadcasterId,
            ReplyToMessageId = messageId
        };
        
        // v1/chat
        var result = await PostAsync<SendChatMessageResponse, SendMessageApiRequest>(ApiUrlPart, ApiVersion.v1, input, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChatWrite}");
        }

        return result;
    }
}