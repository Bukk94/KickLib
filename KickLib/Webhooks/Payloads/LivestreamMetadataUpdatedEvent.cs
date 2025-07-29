namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a livestream metadata updated event.
/// </summary>
public class LivestreamMetadataUpdatedEvent : WebhookEventBase
{
    /// <summary>
    ///     Livestream metadata information.
    /// </summary>
    public LivestreamMetadata Metadata { get; set; } = new();
}