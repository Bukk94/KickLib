using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Messages
{
    public class MessageResponse
    {
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "chat_id")]
        public int ChatId { get; set; }
    
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        public string Content { get; set; }
    
        /// <summary>
        ///     Possible values:
        ///     Message
        ///     Reply
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     MetaData for message.
        ///     For example if Type is Reply, this contains original message (serialized). 
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public string MetaData { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        public MessageSenderResponse Sender { get; set; }
    }
}