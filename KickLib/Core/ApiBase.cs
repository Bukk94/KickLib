using System.Net.Http.Headers;
using KickLib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace KickLib.Core;

public abstract class ApiBase
{
    private readonly ApiSettings _settings;
    private readonly ILogger _logger;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly HttpClient _client;
    
    internal const string BaseUrl = "https://api.kick.com/public/";
    
    protected ApiBase(ApiSettings settings, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);

        _settings = settings;
        _logger = logger;
        _client = new HttpClient();

        _serializerSettings = new JsonSerializerSettings
        {
            Error = delegate(object? sender, ErrorEventArgs args)
            {
                logger.LogError("Deserialization failed in {Sender}! {Ex}", sender?.ToString() ?? "Unknown", args.ErrorContext.Error);

                if (!settings.ThrowOnDeserializationError)
                {
                    args.ErrorContext.Handled = true;
                }
            }
        };
    }
    
    protected async Task<Result<TType>> GetAsync<TType>(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>>? queryParams = null,
        string? accessToken = null)
        where TType : class
    {
        var url = ConstructResourceUrl(urlPart, version, queryParams);

        var token = ResolveAccessToken(accessToken);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.GetAsync(url).ConfigureAwait(false);

        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return HandleErrorResponse((int)response.StatusCode, data, $"GET {url}");
        }

        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<TType>>(data, _serializerSettings);

        return deserializedObject?.Data is not null
            ? Result.Ok(deserializedObject.Data!)
                .WithSuccess(((int)response.StatusCode).ToString())
                .WithSuccess(deserializedObject.Message ?? "Success")
            : HandleErrorResponse((int)response.StatusCode, data, $"GET {url}", $"Failed to deserialize response to type {typeof(TType)}. Received response from url '{url}': {data}");
    }

    private string? ResolveAccessToken(string? accessToken)
    {
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            // We have accessToken override
            return accessToken;
        }

        if (!string.IsNullOrWhiteSpace(_settings.AccessToken))
        {
            // We have accessToken in the settings
            return _settings.AccessToken;
        }
        
        // TODO: We are missing access token - Try to obtain it via App Access Token (once available)
        
        return null;
    }

    private static Result HandleErrorResponse(int statusCode, string data, string targetUrl, string? message = null)
    {
        // Result.Fail(deserializedObject?.Message ?? );
        return Result.Fail(
        [
            message ?? $"Failed to fetch Kick API data. API response: {data}",
            $"Response code: {statusCode}",
            targetUrl
        ]);
    }
    
    private static string ConstructResourceUrl(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>>? queryParams = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(urlPart);

        var url = $"{BaseUrl}v{(int)version}/{urlPart}";

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