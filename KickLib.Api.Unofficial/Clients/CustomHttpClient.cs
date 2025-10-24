using System.Text;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;

namespace KickLib.Api.Unofficial.Clients
{
    /// <summary>
    ///     Currently simple HTTP API call does not work because Kick has several layers of protection and direct API calls are prohibited
    ///     and returns 403 forbidden.
    /// </summary>
    public class CustomHttpClient : IApiCaller
    {
        private readonly HttpClient _httpClient = new();

        /// <inheritdoc />
        public Task<KeyValuePair<int, string>> SendRequestAsync(string url)
        {
            return SendRequestAsync(url, "GET");
        }

        /// <inheritdoc />
        public Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task<KeyValuePair<int, string>> SendAuthenticatedRequestAsync(string url, string payload, HttpMethod? method = null)
        {
            throw new NotImplementedException();
        }

        private async Task<KeyValuePair<int, string>> SendRequestAsync(
            string url,
            string method,
            string payload = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = new HttpMethod(method)
            };

            if (payload != null)
            {
                request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            }

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                return new KeyValuePair<int, string>((int)response.StatusCode, responseContent);
            }

            throw new HttpRequestException("HTTP Request failed: " + responseContent);
        }
    }
}