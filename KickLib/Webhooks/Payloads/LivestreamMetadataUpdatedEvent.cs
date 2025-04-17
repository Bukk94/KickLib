namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a livestream metadata updated event.
/// </summary>
public class LivestreamMetadataUpdatedEvent : WebhookEventBase
{
    /// <summary>
    ///     Livestream metadata information.
    /// </summary>
    public required LivestreamMetadata Metadata { get; set; }
}