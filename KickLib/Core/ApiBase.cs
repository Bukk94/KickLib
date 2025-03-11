using System.Net;
using System.Net.Http.Headers;
using System.Text;
using KickLib.Auth;
using KickLib.Exceptions;
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
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter(typeof(LowerCaseNamingStrategy))
            },
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

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await ExecuteRequestAsync(
            () => _client.GetAsync(url),
            !string.IsNullOrWhiteSpace(accessToken)).ConfigureAwait(false);

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

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        StringContent? payload = null;
        if (input is not null)
        {
            var json = JsonConvert.SerializeObject(input, _serializerSettings);
            payload = new StringContent(json, Encoding.UTF8, "application/json");
        }
        
        var response = await ExecuteRequestAsync(
            () => _client.PostAsync(url, payload),
            !string.IsNullOrWhiteSpace(accessToken)).ConfigureAwait(false);

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

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var json = JsonConvert.SerializeObject(input, _serializerSettings);
        var payload = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await ExecuteRequestAsync(
            () => _client.PatchAsync(url, payload),
            !string.IsNullOrWhiteSpace(accessToken)).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return HandleErrorResponse((int)response.StatusCode, data, $"PATCH {url} | Payload: {json}");
        }

        return Result.Ok(true);
    }
    
    protected async Task<Result<bool>> DeleteAsync(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>>? queryParams = null,
        string? accessToken = null)
    {
        var url = ConstructResourceUrl(urlPart, version, queryParams);

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await ExecuteRequestAsync(
            () => _client.DeleteAsync(url),
            !string.IsNullOrWhiteSpace(accessToken)).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return HandleErrorResponse((int)response.StatusCode, data, $"DELETE {url}");
        }

        return Result.Ok(true);
    }
    
    private async Task<HttpResponseMessage> ExecuteRequestAsync(
        Func<Task<HttpResponseMessage>> requestFunc,
        bool usingExternalToken)
    {
        var response = await requestFunc().ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.Unauthorized &&
            CanRefreshToken(usingExternalToken))
        {
            var token = await RefreshAccessTokenAsync().ConfigureAwait(false);
            // Retry the call if we get new access token
            if (!string.IsNullOrWhiteSpace(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await requestFunc().ConfigureAwait(false);
            }
        }

        return response;
    }

    private async Task<string?> RefreshAccessTokenAsync()
    {
        if (!_settings.CanRefreshToken)
        {
            return null;
        }
        
        // TODO: Obtain it via App Access Token (once available)
        var response = await KickOAuthGenerator.RefreshAccessTokenAsync(
            _settings.RefreshToken!,
            _settings.ClientId!,
            _settings.ClientSecret!).ConfigureAwait(false);

        if (response.IsFailed)
        {
            throw new KickLibException("Failed to refresh access token: " + string.Join(",", response.Errors.Select(x => x.Message)));
        }
        
        _settings.AccessToken = response.Value.AccessToken;
        _settings.RefreshToken = response.Value.RefreshToken;
        _logger.LogInformation("Access token refreshed successfully.");
        
        return response.Value.AccessToken;
    }

    private bool CanRefreshToken(bool usingExternalToken)
    {
        if (usingExternalToken)
        {
            // Do not attempt to refresh token if overload is used
            return false;
        }
        
        // If we have refresh token, we can attempt to refresh the access token
        return !string.IsNullOrWhiteSpace(_settings.RefreshToken);
    }
    
    private async Task<string?> ResolveAccessTokenAsync(string? accessToken)
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

        return await RefreshAccessTokenAsync().ConfigureAwait(false);
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