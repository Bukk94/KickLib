namespace KickLib.Interfaces;

public interface IApiCaller
{
    /// <summary>
    ///     Sends GET request and downloads the content.
    /// </summary>
    /// <param name="url">Url to download / call.</param>
    /// <returns>
    ///     Returns <see cref="KeyValuePair{TKey,TValue}"/>, where Key is HTTP Status Code and Value is JSON data.
    ///     Value must be valid JSON document to be deserialized.
    /// </returns>
    Task<KeyValuePair<int, string>> SendRequestAsync(string url);

    Task AuthenticateAsync(string username, string password);

    Task<KeyValuePair<int, string>> SendAuthenticatedRequestAsync(string url, string payload);
}
