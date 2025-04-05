using KickLib.Models.v1.Chat;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <summary>
///     Chat APIs allow you to use and interact with the chat that is available on the Kick website.
///     You can send a message as a Bot account or your User account.
/// </summary>
public class Chat : ApiBase
{
    private const string ApiUrlPart = "chat";

    /// <inheritdoc />
    public Chat(ApiSettings settings, ILogger logger) : base(settings, logger)
    {
    }
    
    /// <summary>
    ///     Post a chat message to a channel as a user.
    ///     When sending as a user, the broadcaster_user_id is required.
    /// </summary>
    public async Task<Result<SendChatMessageResponse>> SendMessageAsUserAsync(
        int broadcasterId, 
        string message,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        
        var input = new SendMessageRequest(message, MessageType.User)
        {
            BroadcasterId = broadcasterId
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
    
    /// <summary>
    ///     Post a chat message to a channel as a bot.
    ///     As a bot, the message will always be sent to the channel attached to your token.
    /// </summary>
    public async Task<Result<SendChatMessageResponse>> SendMessageAsBotAsync(
        string message,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);

        var input = new SendMessageRequest(message, MessageType.Bot);
        
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