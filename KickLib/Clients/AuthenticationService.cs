using KickLib.Interfaces;
using PuppeteerSharp;

namespace KickLib.Clients;

public class AuthenticationService : IAuthenticationService
{
    public string BearerToken { get; private set; }
    public bool IsAuthenticated => BearerToken is not null;
    
    public async Task AuthenticateAsync(string username, string password)
    {
        throw new NotImplementedException();
    }
    
    private static async Task EnsureBrowserAsync()
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
    }
}