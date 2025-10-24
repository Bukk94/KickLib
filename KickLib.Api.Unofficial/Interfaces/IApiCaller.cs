using KickLib.Api.Unofficial.Models;

namespace KickLib.Api.Unofficial.Interfaces
{
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

        Task AuthenticateAsync(AuthenticationSettings authenticationSettings);

        Task<KeyValuePair<int, string>> SendAuthenticatedRequestAsync(string url, string payload, HttpMethod? method = null);
    }
}
