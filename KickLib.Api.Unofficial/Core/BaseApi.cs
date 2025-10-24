using KickLib.Api.Unofficial.Exceptions;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace KickLib.Api.Unofficial.Core
{
    public abstract class BaseApi
    {
        private const string BaseUrl = $"{Constants.KickUrl}/api/v";
        private readonly IApiCaller _client;
        private readonly ILogger _logger;
        private readonly JsonSerializerSettings _serializerSettings;

        /// <summary>
        ///     When set to true, API will throw an exception when deserialization fails.
        ///     Default: false
        ///     Enabling this might cause instability as Kick API is not official and they might change some fields in API without notice.
        /// </summary>
        public static bool ThrowOnDeserializationError { get; set; }
    
        protected BaseApi(IApiCaller client, ILogger logger = null)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;

            _serializerSettings = new JsonSerializerSettings
            {
                Error = delegate(object sender, ErrorEventArgs args)
                {
                    logger?.LogError("Deserialization failed in {sender}! {ex}", sender, args.ErrorContext.Error);

                    if (!ThrowOnDeserializationError)
                    {
                        args.ErrorContext.Handled = true;
                    }
                }
            };
        }
    
        protected async Task<TType> GetAsync<TType>(
            string urlPart,
            ApiVersion version,
            List<KeyValuePair<string, string>> queryParams = null)
        {
            var url = ConstructResourceUrl(urlPart, version, queryParams);

            var data = await _client.SendRequestAsync(url).ConfigureAwait(false);
            if (data.Key != 200 && 
                data.Key != 204)
            {
                HandleErrorThrowIfCritical(data.Key);
                return default;
            }
        
            return JsonConvert.DeserializeObject<TType>(data.Value, _serializerSettings);
        }
    
        protected async Task<TType> GetAsync<TType>(
            string urlPart,
            ApiVersion version,
            string part,
            List<KeyValuePair<string, string>> queryParams = null)
        {
            var url = ConstructResourceUrl(urlPart, version, queryParams);

            var data = await _client.SendRequestAsync(url).ConfigureAwait(false);
            if (data.Key != 200 && 
                data.Key != 204)
            {
                HandleErrorThrowIfCritical(data.Key);
                return default;
            }
        
            var parsed = JObject.Parse(data.Value);
            var partData = parsed.SelectToken(part)?.ToString();
            return partData is not null
                ? JsonConvert.DeserializeObject<TType>(partData, _serializerSettings)
                : default;
        }

        protected async Task<TType> GetAuthenticatedAsync<TType>(
            string urlPart,
            ApiVersion version)
        {
            var url = ConstructResourceUrl(urlPart, version);

            var data = await _client.SendAuthenticatedRequestAsync(url, null).ConfigureAwait(false);
            if (data.Key != 200 && 
                data.Key != 204)
            {
                HandleErrorThrowIfCritical(data.Key);
                return default;
            }
        
            return JsonConvert.DeserializeObject<TType>(data.Value, _serializerSettings);
        }
    
        protected async Task<TType?> PostAuthenticatedAsync<TType>(
            string urlPart,
            ApiVersion version,
            object payload)
        {
            if (payload is null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
        
            var url = ConstructResourceUrl(urlPart, version);
            var payloadJson = JsonConvert.SerializeObject(payload);

            var data = await _client.SendAuthenticatedRequestAsync(url, payloadJson);
            if (data.Key != 200 && 
                data.Key != 204)
            {
                HandleErrorThrowIfCritical(data.Key);
                return default;
            }
        
            return JsonConvert.DeserializeObject<TType>(data.Value, _serializerSettings);
        }
        
        protected async Task<bool> DeleteAuthenticatedAsync(
            string urlPart,
            ApiVersion version)
        {
            var url = ConstructResourceUrl(urlPart, version);
            var result = await _client.SendAuthenticatedRequestAsync(url, null, HttpMethod.Delete);
            
            return result.Key is 200 or 204;
        }
    
        private static string ConstructResourceUrl(
            string urlPart,
            ApiVersion version,
            List<KeyValuePair<string, string>> queryParams = null)
        {
            if (urlPart == null)
            {
                throw new Exception("Cannot pass null resource with null override url");
            }

            var url = version switch
            {
                ApiVersion.None => $"{Constants.KickUrl}/{urlPart}",
                ApiVersion.V1Internal => $"{Constants.KickUrl}/api/internal/v1/{urlPart}",
                _ => $"{BaseUrl}{(int)version}/{urlPart}"
            };

            if (queryParams != null)
            {
                var first = true;
                foreach (var query in queryParams)
                {
                    var symbol = first ? "?" : "&";
                    url += $"{symbol}{query.Key}={Uri.EscapeDataString(query.Value)}";
                    first = false;
                }
            }

            return url;
        }

        private static void HandleErrorThrowIfCritical(int errorCode)
        {
            switch (errorCode)
            {
                case 500: throw new KickLibException("KickLib failed to get data from Kick.com");
                case 503: throw new KickUnavailableException();
            }
        }
    }
}