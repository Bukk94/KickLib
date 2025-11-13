using System.Diagnostics;
using System.Text;
using KickLib;
using KickLib.Auth;
using System.Net;

namespace KickLib.Examples;

/// <summary>
///     Local OAuth process example for Kick.com.
///     Uses HttpListener to capture the redirect after user authentication.
///
///     This allows you to perform full OAuth process locally and to obtain access and refresh tokens.
///     This is useful for development and testing purposes, but should not be used in any production or client application
///     (as it requires client secret to be stored locally, which is NOT secure).
///
///     REQUIREMENTS:
///     * You must have a registered Kick application
///     * Client ID and Client Secret from the application
///     * Redirect URL must match the one registered in the application
///
///     Usage:
///     var tokens = await LocalOAuth.PerformKickLoginAsync([
///         KickScopes.ChannelRead,
///         KickScopes.ChatWrite,
///         KickScopes.UserRead,
///         KickScopes.KicksRead
///     ]);
/// </summary>
public static class LocalOAuth
{
    // Get these from your application: https://kick.com/settings/developer
    private const string ClientId = "XXXXXXXXXXXXXXXXXX";
    private const string ClientSecret = "YYYYYYYYYYYYYYYYYYYYY";
    // Allow following URL in the application as 'Redirect URL'
    private const string RedirectUrl = "http://localhost:5000";
    
    public static async Task<KickTokenResponse?> PerformKickLoginAsync(ICollection<string> scopes)
    {
        var gen = new KickOAuthGenerator();
        
        var authorizationUrl = gen.GetAuthorizationUri(RedirectUrl, ClientId, scopes, out var verifier).ToString();

        // Start listener
        using var listener = new HttpListener();
        listener.Prefixes.Add(RedirectUrl);
        listener.Start();

        Console.WriteLine("Opening browser for authentication...");
        Process.Start(new ProcessStartInfo(authorizationUrl) { UseShellExecute = true });

        Console.WriteLine("Waiting for redirect...");
        var context = await listener.GetContextAsync();

        // Send a response to browser
        var response = context.Response;
        var responseString = "<html><body>You can close this window now.</body></html>";
        var buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer);
        response.OutputStream.Close();

        // Get code from query
        var request = context.Request;
        var code = request.QueryString["code"];
        var state = request.QueryString["state"];

        if (string.IsNullOrEmpty(state) || string.IsNullOrEmpty(code))
        {
            Console.WriteLine("Invalid response.");
            return null;
        }

        Console.WriteLine("Authorization code received.");

        // Exchange code for token
        var token = await gen.ExchangeCodeForTokenAsync(code, ClientId, ClientSecret, RedirectUrl, state, verifier);
        
        if (token.IsSuccess)
        {
            Console.WriteLine("Successfully generated!");
            Console.WriteLine("Access token:");
            Console.WriteLine(token.Value.AccessToken);
            Console.WriteLine("Refresh token:");
            Console.WriteLine(token.Value.RefreshToken);
        }
        else
        {
            Console.WriteLine("Failed to obtain token");
        }
        
        return token.ValueOrDefault;
    }
}