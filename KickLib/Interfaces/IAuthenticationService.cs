using Microsoft.Extensions.Primitives;
using PuppeteerSharp;

namespace KickLib.Interfaces;

public interface IAuthenticationService
{
    string BearerToken { get; }
    string XsrfToken { get; }
    public bool IsAuthenticated { get; }

    public Task AuthenticateAsync(string username, string password, string totp);
    public Task RefreshXsrfTokenAsync(IPage targetPage);
}