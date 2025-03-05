namespace KickLib.Webhooks.Payloads;

public class ChatMessageSentEvent : WebhookEventBase
{
    public required string MessageId { get; set; }
    
    public required KickUser Sender { get; set; }
    
    public required string Content { get; set; }
    
    public ICollection<Emotes>? Emotes { get; set; }
}