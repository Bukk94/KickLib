using KickLib.Interfaces;
using PuppeteerSharp;

namespace KickLib.Clients;

public class AuthenticationService : IAuthenticationService
{
    public string BearerToken { get; private set; }
    public bool IsAuthenticated => BearerToken is not null;
    
    public Task AuthenticateAsync(string username, string password)
    {
        // TODO: Will be implemented soon.
        // For now anyone can implement own auth flow and pass it into Client using IAuthenticationService
        throw new NotImplementedException();
    }
    
    private static async Task EnsureBrowserAsync()
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
    }
}