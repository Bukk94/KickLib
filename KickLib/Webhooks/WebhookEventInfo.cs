using System.Security.Cryptography;
using System.Text;
using KickLib.Extensions;
using KickLib.Models.v1.EventSubscriptions;

namespace KickLib.Webhooks;

/// <summary>
///     Contains Webhook response event information.
/// </summary>
public class WebhookEventInfo
{
    private static readonly RSA DefaultPublicKey = ParsePublicKey(KickPublicKey);

    /// <summary>
    ///     Event Type Name.
    /// </summary>
    public string EventTypeName { get; }
    
    /// <summary>
    ///     Event Type.
    /// </summary>
    public EventType EventType { get; }
    
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
    public string MessageTimestamp { get; }
    
    /// <summary>
    ///     Signature to verify the sender in base64.
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
    
    /// <summary>
    ///     Creates a new instance of <see cref="WebhookEventInfo"/>.
    /// </summary>
    public WebhookEventInfo(
        string eventTypeName, 
        int eventVersion, 
        string messageTimestamp, 
        string signature, 
        string subscriptionId, 
        string messageId)
    {
        EventTypeName = eventTypeName;
        EventType = eventTypeName.ToEventType();
        EventVersion = eventVersion;
        MessageTimestamp = messageTimestamp;
        Signature = signature;
        SubscriptionId = subscriptionId;
        MessageId = messageId;
    }
    
    /// <summary>
    ///     Validates the sender signature against Kick Public Key.
    ///     When no signature key is provided, default Kick Public Key will be used.
    /// </summary>
    /// <param name="payload">Webhook event payload to validate.</param>
    /// <param name="publicKeySignature">Public Key to use for validation. If null, default Kick Public Key will be used.</param>
    /// <returns>Returns true if payload is from valid Kick sender.</returns>
    public bool ValidateSender(string payload, string? publicKeySignature = null)
    {
        var publicKey = !string.IsNullOrWhiteSpace(publicKeySignature) 
            ? ParsePublicKey(publicKeySignature) 
            : DefaultPublicKey;

        var payloadBytes = Encoding.UTF8.GetBytes($"{MessageId}.{MessageTimestamp}.{payload}");
#if NET8_0_OR_GREATER
        var hashedPayload = SHA256.HashData(payloadBytes);
#else
        byte[] hashedPayload;
        using (var sha256 = SHA256.Create())
        {
            hashedPayload = sha256.ComputeHash(payloadBytes);
        }
#endif

        try
        {
            var signature = Convert.FromBase64String(Signature);
            return publicKey.VerifyHash(hashedPayload, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return false;
        }
    }
    
    private static RSA ParsePublicKey(string signatureKey)
    {
        try
        {
            var rsa = RSA.Create();
#if NET8_0_OR_GREATER
            rsa.ImportFromPem(signatureKey);
#else
            // Parse PEM manually for .NET Standard 2.1
            var base64 = signatureKey
                .Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Trim();
            
            var keyBytes = Convert.FromBase64String(base64);
            rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
#endif
            return rsa;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to parse public key", ex);
        }
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