namespace KickLib.Webhooks.Payloads;

public abstract class WebhookEventBase
{
    public required KickUser Broadcaster { get; set; }
}