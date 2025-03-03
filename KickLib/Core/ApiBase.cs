using System.Net.Http.Headers;
using System.Text;
using KickLib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace KickLib.Core;

public abstract class ApiBase
{
    private readonly ApiSettings _settings;
    private readonly ILogger _logger;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly HttpClient _client;

    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = new List<JsonConverter>
        {
            new StringEnumConverter(typeof(LowerCaseNamingStrategy))
        }
    };
    
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

    protected Task<Result<TType>> PostAsync<TType>(
        string urlPart,
        ApiVersion version,
        string? accessToken = null)
        where TType : class
    {
        return PostAsync<TType, object>(urlPart, version, null, accessToken);
    }
    
    protected async Task<Result<TType>> PostAsync<TType, TInput>(
        string urlPart,
        ApiVersion version,
        TInput? input,
        string? accessToken = null)
        where TType : class
        where TInput : class
    {
        var url = ConstructResourceUrl(urlPart, version);

        var token = ResolveAccessToken(accessToken);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        StringContent? payload = null;
        if (input is not null)
        {
            var json = JsonConvert.SerializeObject(input, SerializerSettings);
            payload = new StringContent(json, Encoding.UTF8, "application/json");
        }
        
        var response = await _client.PostAsync(url, payload).ConfigureAwait(false);

        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return HandleErrorResponse((int)response.StatusCode, data, $"POST {url}");
        }

        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<TType>>(data, _serializerSettings);

        return deserializedObject?.Data is not null
            ? Result.Ok(deserializedObject.Data!)
                .WithSuccess(((int)response.StatusCode).ToString())
                .WithSuccess(deserializedObject.Message ?? "Success")
            : HandleErrorResponse((int)response.StatusCode, data, $"POST {url}", $"Failed to deserialize response to type {typeof(TType)}. Received response from url '{url}': {data}");
    }

    protected async Task<Result<bool>> PatchAsync<TType>(
        string urlPart,
        ApiVersion version,
        TType input,
        string? accessToken = null)
        where TType : class
    {
        var url = ConstructResourceUrl(urlPart, version);

        var token = ResolveAccessToken(accessToken);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var json = JsonConvert.SerializeObject(input, SerializerSettings);
        var payload = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PatchAsync(url, payload).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return HandleErrorResponse((int)response.StatusCode, data, $"PATCH {url} | Payload: {json}");
        }

        return Result.Ok(true);
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
        List<string> errors =
        [
            message ?? $"Failed to fetch Kick API data. API response: {data}",
            $"Response code: {statusCode}",
            targetUrl
        ];

        if (statusCode == 401)
        {
            errors.Add("Unauthorized. Please check your access token. It might be invalid or expired.");
        }

        return Result.Fail(errors);
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