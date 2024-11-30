using KickLib.Clients.CycleTls;
using KickLib.Exceptions;
using KickLib.Interfaces;
using KickLib.Models;
using Microsoft.Extensions.Logging;
using Polly;

namespace KickLib.Clients;

/// <summary>
///     Client that uses TLS/JA3 Fingerprint spoofing to impersonate user.
///     This client effectively bypasses most of the Kick's API protections.
/// </summary>
public class TlsSpoofClient : IApiCaller
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger _logger;

    public TlsSpoofClient(
        IAuthenticationService authenticationService,
        SpoofSettings settings = null,
        ILogger logger = null)
    {
        _authenticationService = authenticationService;
        _logger = logger;
        
        CycleTlsInitializer.Initialize(settings, logger);
    }

    /// <inheritdoc />
    public Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
    {
        return _authenticationService.AuthenticateAsync(authenticationSettings);
    }

    /// <inheritdoc />
    public async Task<KeyValuePair<int, string>> SendRequestAsync(string url)
    {
        try
        {
            var options = CycleTlsInitializer.GetOptions(url);
            var response = await CycleTlsInitializer.Client.SendAsync(options);

            return new KeyValuePair<int, string>(response.Status, response.Body);
        }
        catch (Exception ex)
        {
            throw new KickLibException("KickLib failed to get response from Kick.com. See inner exception for details.", ex);
        }
    }

    /// <inheritdoc />
    public async Task<KeyValuePair<int, string>> SendAuthenticatedRequestAsync(string url, string payload)
    {
        if (!_authenticationService.IsAuthenticated)
        {
            throw new ArgumentException($"Cannot send authenticated request without authenticating first! Call '{nameof(AuthenticateAsync)}' first.");
        }
       
        try
        {
            var options = CycleTlsInitializer.GetOptions(url);

            var method = payload is not null ? "POST" : "GET";

            options.Method = method;
            options.Body = payload;
            options.Headers.TryAdd("X-XSRF-TOKEN", _authenticationService.XsrfToken);
            options.Headers.TryAdd("Authorization", $"Bearer {_authenticationService.BearerToken}");

            CycleTlsResponse response = null;
            await Policy
                .Handle<XsrfMismatchException>()
                .RetryAsync(2, async (_, _) =>
                {
                    // Refresh Xsrf token if we get a mismatch
                    await _authenticationService.RefreshXsrfTokenAsync(url);
                })
                .ExecuteAsync(async () =>
                {
                    response = await CycleTlsInitializer.Client.SendAsync(options);
                });

            if (response is null)
            {
                throw new ArgumentException("Couldn't get the response from target page");
            }

            return new KeyValuePair<int, string>(response.Status, response.Body);
        }
        catch (Exception ex)
        {
            _logger?.LogError($"Error sending message: {ex.Message}");
        }

        // At this point we got error, so return empty string with 500.
        return new KeyValuePair<int, string>(500, string.Empty);
    }
}