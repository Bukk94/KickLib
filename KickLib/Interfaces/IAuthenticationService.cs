using KickLib.Models;
using PuppeteerSharp;

namespace KickLib.Interfaces;

public interface IAuthenticationService
{
    string BearerToken { get; }
    string XsrfToken { get; }
    public bool IsAuthenticated { get; }

    public Task AuthenticateAsync(AuthenticationSettings authenticationSettings);
    public Task RefreshXsrfTokenAsync(IPage targetPage);
}