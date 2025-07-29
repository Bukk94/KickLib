namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents metadata for a ban or timeout.
/// </summary>
public class BanMetadata
{
    /// <summary>
    ///     Reason of the ban.
    /// </summary>
    public string Reason { get; set; } = string.Empty;
    
    /// <summary>
    ///     When the ban (timeout) was issued.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    ///     When ban (timeout) expires.
    ///     If <c>null</c>, ban is permanent.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}