namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Base information for all webhook events.
/// </summary>
public abstract class WebhookEventBase
{
    /// <summary>
    ///     Details about the broadcaster user for this webhook event.
    /// </summary>
    public KickUser Broadcaster { get; set; } = new();
}