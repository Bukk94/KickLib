using System.Net;
using System.Net.Http.Headers;
using System.Text;
using KickLib.Auth;
using KickLib.Exceptions;
using KickLib.Extensions;
using KickLib.Models;
using KickLib.Models.Errors;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace KickLib.Core;

/// <summary>
///     Base implementation for all API calls via HTTP Client.
/// </summary>
public abstract class ApiBase
{
    private readonly ApiSettings _settings;
    private readonly ILogger _logger;
    private readonly JsonSerializerSettings _serializerSettings;
    private readonly IKickOAuthGenerator _kickOAuthGenerator;
    private readonly IHttpClientFactory _clientFactory;

    private const string BaseUrl = "https://api.kick.com/public/";
    
    /// <summary>
    ///     Base constructor.
    /// </summary>
    /// <param name="settings">API Settings class.</param>
    /// <param name="oauthGenerator">Manages OAuth tokens.</param>
    /// <param name="clientFactory">Http Client Factory</param>
    /// <param name="logger">Instance of a logger.</param>
    protected ApiBase(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger logger)
    {
        if (settings is null)
        {
            throw new ArgumentNullException(nameof(settings));
        }
        
        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }
 
        _settings = settings;
        _logger = logger;
        _kickOAuthGenerator = oauthGenerator;
        _clientFactory = clientFactory;

        _serializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters =
            [
                new StringEnumConverter(typeof(LowerCaseNamingStrategy))
            ],
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
    
    /// <summary>
    ///     Perform GET request.
    /// </summary>
    protected async Task<Result<TType>> GetAsync<TType>(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>>? queryParams = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TType : class
    {
        var url = ConstructResourceUrl(urlPart, version, queryParams);

        var client = GetClient();
        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        
        var response = await ExecuteRequestAsync(
            () => client.GetAsync(url, cancellationToken),
            !string.IsNullOrWhiteSpace(accessToken),
            client).ConfigureAwait(false);

        var (data, error) = await ProcessResponseAsync(response, $"GET {url}").ConfigureAwait(false);
        if (data is null ||
            response is null)
        {
            return error;
        }

        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<TType>>(data, _serializerSettings);

        return deserializedObject?.Data is not null
            ? Result.Ok(deserializedObject.Data!)
                .WithSuccess(((int)response.StatusCode).ToString())
                .WithSuccess(deserializedObject.Message ?? "Success")
            : HandleErrorResponse(response, data, $"GET {url}", $"Failed to deserialize response to type {typeof(TType)}. Received response from url '{url}': {data}");
    }
    
    /// <summary>
    ///     Perform POST request without payload
    /// </summary>
    protected Task<Result<TType>> PostAsync<TType>(
        string urlPart,
        ApiVersion version,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TType : class
    {
        return PostAsync<TType, object>(urlPart, version, null, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Perform POST request with input payload.
    /// </summary>
    protected async Task<Result<TType>> PostAsync<TType, TInput>(
        string urlPart,
        ApiVersion version,
        TInput? input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TType : class
        where TInput : class
    {
        var url = ConstructResourceUrl(urlPart, version);

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        StringContent? payload = null;
        if (input is not null)
        {
            var json = JsonConvert.SerializeObject(input, _serializerSettings);
            payload = new StringContent(json, Encoding.UTF8, "application/json");
        }
        
        var response = await ExecuteRequestAsync(
            () => client.PostAsync(url, payload, cancellationToken),
            !string.IsNullOrWhiteSpace(accessToken),
            client).ConfigureAwait(false);
        
        var (data, error) = await ProcessResponseAsync(response, $"POST {url}").ConfigureAwait(false);
        if (data is null ||
            response is null)
        {
            return error;
        }

        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<TType>>(data, _serializerSettings);

        return deserializedObject?.Data is not null
            ? Result.Ok(deserializedObject.Data!)
                .WithSuccess(((int)response.StatusCode).ToString())
                .WithSuccess(deserializedObject.Message ?? "Success")
            : HandleErrorResponse(response, data, $"POST {url}", $"Failed to deserialize response to type {typeof(TType)}. Received response from url '{url}': {data}");
    }

    /// <summary>
    ///     Perform PATCH request.
    /// </summary>
    protected async Task<Result<bool>> PatchAsync<TType>(
        string urlPart,
        ApiVersion version,
        TType input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TType : class
    {
        var url = ConstructResourceUrl(urlPart, version);

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var json = JsonConvert.SerializeObject(input, _serializerSettings);
        var payload = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await ExecuteRequestAsync(
            () => client.PatchAsync(url, payload, cancellationToken),
            !string.IsNullOrWhiteSpace(accessToken),
            client).ConfigureAwait(false);
        
        if (response is null)
        {
            return Result.Fail("Request was cancelled via CancellationToken."); 
        }
        
        if (!response.IsSuccessStatusCode)
        {
#if NET8_0_OR_GREATER
            var data = await response.Content.ReadAsStringAsync(CancellationToken.None).ConfigureAwait(false);
#else
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif
            return HandleErrorResponse(response, data, $"PATCH {url} | Payload: {json}");
        }

        return Result.Ok(true);
    }
    
    /// <summary>
    ///     Perform PATCH request.
    /// </summary>
    protected async Task<Result<TType>> PatchAsync<TType, TInput>(
        string urlPart,
        ApiVersion version,
        TInput input,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TType : class
    {
        var url = ConstructResourceUrl(urlPart, version);

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var json = JsonConvert.SerializeObject(input, _serializerSettings);
        var payload = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await ExecuteRequestAsync(
            () => client.PatchAsync(url, payload, cancellationToken),
            !string.IsNullOrWhiteSpace(accessToken),
            client).ConfigureAwait(false);
        
        var (data, error) = await ProcessResponseAsync(response, $"PATCH {url}").ConfigureAwait(false);
        if (data is null ||
            response is null)
        {
            return error;
        }
        
        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<TType>>(data, _serializerSettings);

        return deserializedObject?.Data is not null
            ? Result.Ok(deserializedObject.Data!)
                .WithSuccess(((int)response.StatusCode).ToString())
                .WithSuccess(deserializedObject.Message ?? "Success")
            : HandleErrorResponse(response, data, $"PATCH {url}", $"Failed to deserialize response to type {typeof(TType)}. Received response from url '{url}': {data}");
    }

    /// <summary>
    ///     Perform DELETE request.
    /// </summary>
    protected Task<Result<bool>> DeleteAsync(
        string urlPart,
        ApiVersion version,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        return DeleteAsync(urlPart, version, null, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Perform DELETE request with query params.
    /// </summary>
    protected async Task<Result<bool>> DeleteAsync(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>>? queryParams = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var url = ConstructResourceUrl(urlPart, version, queryParams);

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }
        
        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await ExecuteRequestAsync(
            () => client.DeleteAsync(url, cancellationToken),
            !string.IsNullOrWhiteSpace(accessToken),
            client).ConfigureAwait(false);

        if (response is null)
        {
            return Result.Fail("Request was cancelled via CancellationToken."); 
        }
        
        if (!response.IsSuccessStatusCode)
        {
#if NET8_0_OR_GREATER
            var data = await response.Content.ReadAsStringAsync(CancellationToken.None).ConfigureAwait(false);
#else
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif
            return HandleErrorResponse(response, data, $"DELETE {url}");
        }

        return Result.Ok(true);
    }
    
    /// <summary>
    ///     Perform DELETE request with payload.
    /// </summary>
    protected async Task<Result<bool>> DeleteAsync<TPayload>(
        string urlPart,
        ApiVersion version,
        TPayload? input = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
        where TPayload : class
    {
        var url = ConstructResourceUrl(urlPart, version);

        var token = await ResolveAccessTokenAsync(accessToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Fail("Access token is missing.");
        }

        var client = GetClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        StringContent? payload = null;
        if (input is not null)
        {
            var json = JsonConvert.SerializeObject(input, _serializerSettings);
            payload = new StringContent(json, Encoding.UTF8, "application/json");
        }
        
        var response = await ExecuteRequestAsync(
            () => client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = payload
            }, cancellationToken),
            !string.IsNullOrWhiteSpace(accessToken),
            client).ConfigureAwait(false);

        if (response is null)
        {
            return Result.Fail("Request was cancelled via CancellationToken."); 
        }
        
        if (!response.IsSuccessStatusCode)
        {
#if NET8_0_OR_GREATER
            var data = await response.Content.ReadAsStringAsync(CancellationToken.None).ConfigureAwait(false);
#else
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif
            return HandleErrorResponse(response, data, $"DELETE {url}");
        }

        return Result.Ok(true);
    }
    
    private async Task<HttpResponseMessage?> ExecuteRequestAsync(
        Func<Task<HttpResponseMessage>> requestFunc,
        bool usingExternalToken,
        HttpClient client)
    {
        HttpResponseMessage response;
        try
        {
            response = await requestFunc().ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            return default;
        }
        
        if (response.StatusCode == HttpStatusCode.Unauthorized &&
            CanRefreshToken(usingExternalToken))
        {
            var token = await RefreshAccessTokenAsync().ConfigureAwait(false);
            // Retry the call if we get new access token
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                try
                {
                    response = await requestFunc().ConfigureAwait(false);
                } 
                catch (TaskCanceledException)
                {
                    return default;
                }
            }
        }

        return response;
    }

    private static async Task<(string? Data, Result? Error)> ProcessResponseAsync(
        HttpResponseMessage? response,
        string origin)
    {
        if (response is null)
        {
            return (null, Result.Fail("Request was cancelled via CancellationToken.")); 
        }
        
        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return (null, HandleErrorResponse(response, data, origin));
        }

        return (data, null);
    }
    
    private async Task<string?> RefreshAccessTokenAsync()
    {
        if (!_settings.CanRefreshToken)
        {
            return null;
        }
        
        var response = await _kickOAuthGenerator.RefreshAccessTokenAsync(
            _settings.RefreshToken!,
            _settings.ClientId!,
            _settings.ClientSecret!).ConfigureAwait(false);

        if (response.IsFailed)
        {
            var error = response.GetResponseError()!;
            throw new KickLibRefreshTokenException(
                "Failed to refresh access token: " + string.Join(",", response.Errors.Select(x => x.Message)),
                error.HttpResponseMessage);
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

    private static Result HandleErrorResponse(HttpResponseMessage response, string data, string targetUrl, string? message = null)
    {
        var statusCode = (int)response.StatusCode;
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

        return Result.Fail(errors)
            .WithError(new KickLibHttpResponseError(response));
    }
    
    private static string ConstructResourceUrl(
        string urlPart,
        ApiVersion version,
        List<KeyValuePair<string, string>>? queryParams = null)
    {
        if (string.IsNullOrWhiteSpace(urlPart))
        {
            throw new ArgumentException("URL part cannot be null or empty.", nameof(urlPart));
        }

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

    private HttpClient GetClient()
    {
        return _clientFactory.CreateClient(HttpConstants.HttpClientName);
    }
}