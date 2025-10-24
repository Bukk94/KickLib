using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models.Response;
using KickLib.Api.Unofficial.Models.Response.v2.Messages;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Api
{
    /// <summary>
    ///     Allows sending messages to the chat.
    /// </summary>
    public class Messages : BaseApi
    {
        private const string ApiUrlPart = "messages/";

        public Messages(IApiCaller client, ILogger logger = null)
            : base(client, logger)
        {
        }

        /// <summary>
        ///     [Auth Required] Sends message to chatroom.
        /// </summary>
        /// <param name="chatroomId">Chatroom ID where to send the message.</param>
        /// <param name="message">Message to be send.</param>
        /// <returns>Returns response object (if successful), containing message ID and other details.</returns>
        public async Task<SendMessageResponse?> SendMessageAsync(int chatroomId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }
        
            var urlPart = $"{ApiUrlPart}send/{chatroomId}";

            var payload = new
            {
                content = message,
                type = "message"
            };
        
            var messageData = await PostAuthenticatedAsync<DataWrapper<SendMessageResponse>>(urlPart, ApiVersion.V2, payload);
            return messageData?.Data;
        }
        
        /// <summary>
        ///     [Auth Required] Deletes a specific message from a chatroom.
        /// </summary>
        /// <param name="chatroomId">Chatroom ID in which delete the message.</param>
        /// <param name="messageId">Message ID to be deleted.</param>
        public async Task<bool> DeleteMessageAsync(int chatroomId, string messageId)
        {
            if (string.IsNullOrWhiteSpace(messageId))
            {
                throw new ArgumentNullException(nameof(messageId));
            }
        
            var urlPart = $"chatrooms/{chatroomId}/messages/{messageId}";

            return await DeleteAuthenticatedAsync(urlPart, ApiVersion.V2);
        }
        
        /// <summary>
        ///     [Auth Required] Deletes a messages in bulk from a chatroom.
        /// </summary>
        /// <param name="chatroomId">Chatroom ID in which delete the message.</param>
        /// <param name="messageIds">Message IDs to be deleted.</param>
        public async Task DeleteMessagesAsync(int chatroomId, ICollection<string> messageIds)
        {
            if (!messageIds.Any())
            {
                return;
            }
            
            foreach (var messageId in messageIds.Distinct().Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                await DeleteMessageAsync(chatroomId, messageId);
            }
        }
    }
}