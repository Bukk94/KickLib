namespace KickLib.Webhooks;

public class WebhookEventInfo
{
    /// <summary>
    ///     Event Type.
    /// </summary>
    public string EventType { get; }
    
    /// <summary>
    ///     Event version.
    /// </summary>
    public int EventVersion { get; }
    
    /// <summary>
    ///     Timestamp of when the message was sent.
    /// </summary>
    /// <remarks>
    ///     RFC3339 Date-time
    /// </remarks>
    public DateTimeOffset MessageTimestamp { get; }
    
    /// <summary>
    ///     Signature to verify the sender
    /// </summary>
    public string Signature { get; }
    
    /// <summary>
    ///     Subscription ID associated with event.
    /// </summary>
    public string SubscriptionId { get; }
    
    /// <summary>
    ///     Unique message ID, idempotent key.
    /// </summary>
    public string MessageId { get; }
    
    public WebhookEventInfo(
        string eventType, 
        int eventVersion, 
        DateTimeOffset messageTimestamp, 
        string signature, 
        string subscriptionId, 
        string messageId)
    {
        EventType = eventType;
        EventVersion = eventVersion;
        MessageTimestamp = messageTimestamp;
        Signature = signature;
        SubscriptionId = subscriptionId;
        MessageId = messageId;
    }

    /// <summary>
    ///     Default Kick Public Key for sender validation.
    ///     For always up-to-date key, use v1/public-key (or _api.Authorization.GetPublicKeyAsync()).
    /// </summary>
    public const string KickPublicKey = 
@"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAq/+l1WnlRrGSolDMA+A8
6rAhMbQGmQ2SapVcGM3zq8ANXjnhDWocMqfWcTd95btDydITa10kDvHzw9WQOqp2
MZI7ZyrfzJuz5nhTPCiJwTwnEtWft7nV14BYRDHvlfqPUaZ+1KR4OCaO/wWIk/rQ
L/TjY0M70gse8rlBkbo2a8rKhu69RQTRsoaf4DVhDPEeSeI5jVrRDGAMGL3cGuyY
6CLKGdjVEM78g3JfYOvDU/RvfqD7L89TZ3iN94jrmWdGz34JNlEI5hqK8dd7C5EF
BEbZ5jgB8s8ReQV8H+MkuffjdAj3ajDDX3DOJMIut1lBrUVD1AaSrGCKHooWoL2e
twIDAQAB
-----END PUBLIC KEY-----";
}