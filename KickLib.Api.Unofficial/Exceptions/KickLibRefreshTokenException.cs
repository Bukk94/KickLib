namespace KickLib.Exceptions;

/// <summary>
///     Exception thrown when the refresh token is invalid or expired.
///     Contains original HTTP response message for further inspection.
/// </summary>
public class KickLibRefreshTokenException : KickLibException
{
    public HttpResponseMessage HttpResponseMessage { get; }
    
    public KickLibRefreshTokenException(string message, HttpResponseMessage httpResponseMessage)
        : base(message)
    {
        HttpResponseMessage = httpResponseMessage ?? throw new ArgumentNullException(nameof(httpResponseMessage));
    }
}