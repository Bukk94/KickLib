using KickLib.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KickLib.Core;

public abstract class BaseApi
{
    internal const string BaseUrl = "https://kick.com/api/v";
    private readonly IApiCaller _client;

    protected BaseApi(IApiCaller client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    
    protected async Task<TType> GetAsync<TType>(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>> queryParams = null)
    {
        var url = ConstructResourceUrl(urlPart, version, queryParams);

        var data = await _client.SendRequestAsync(url).ConfigureAwait(false);

        return JsonConvert.DeserializeObject<TType>(data.Value);
    }
    
    protected async Task<TType> GetAsync<TType>(
        string urlPart,
        ApiVersion version,
        string part,
        List<KeyValuePair<string, string>> queryParams = null)
    {
        var url = ConstructResourceUrl(urlPart, version, queryParams);

        var data = await _client.SendRequestAsync(url).ConfigureAwait(false);

        var parsed = JObject.Parse(data.Value);
        var partData = parsed.SelectToken(part)?.ToString();
        return partData is not null
            ? JsonConvert.DeserializeObject<TType>(partData)
            : default;
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

        var url = $"{BaseUrl}{(int)version}/{urlPart}";

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
}