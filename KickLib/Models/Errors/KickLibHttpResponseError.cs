namespace KickLib.Models.Errors;

/// <summary>
///     KickLib HTTP response error containing metadata about the HTTP response that caused the error.
/// </summary>
public class KickLibHttpResponseError : IError
{
    /// <summary>
    ///     Error message.
    /// </summary>
    public string Message { get; }
    
    /// <summary>
    ///     Metadata about the response.
    /// </summary>
    public Dictionary<string, object> Metadata { get; }

    /// <summary>
    ///     UNUSED
    /// </summary>
    public List<IError> Reasons { get; } = new();

    /// <summary>
    ///     Original HTTP response message that caused the error.
    /// </summary>
    public HttpResponseMessage HttpResponseMessage { get; }

    /// <summary>
    ///     Constructs the Error response object.
    /// </summary>
    public KickLibHttpResponseError(string message, HttpResponseMessage responseMessage)
    {
        Message = message;
        HttpResponseMessage = responseMessage;
        Metadata = new Dictionary<string, object>
        {
            { "StatusCode", (int)responseMessage.StatusCode },
            { "Headers", responseMessage.Headers.ToString() }
        };
    }

    /// <summary>
    ///     Constructs the Error response object.
    /// </summary>
    public KickLibHttpResponseError(HttpResponseMessage responseMessage)
        :this("Unsuccessful response from Kick API", responseMessage)
    {
    }
}