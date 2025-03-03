namespace KickLib.Core;

public class ApiSettings
{
    public string? AccessToken { get; set; }
    
    /// <summary>
    ///     When set to true, API will throw an exception when deserialization fails.
    ///     Default: false
    ///     Enabling this might cause instability as Kick API is not official and they might change some fields in API without notice.
    /// </summary>
    public bool ThrowOnDeserializationError { get; set; }
}