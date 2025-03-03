using KickLib.Api.Unofficial.Models;

namespace KickLib.Api.Unofficial.Interfaces
{
    public interface IAuthenticationService
    {
        string BearerToken { get; }
        string XsrfToken { get; }
        public bool IsAuthenticated { get; }

        public Task AuthenticateAsync(AuthenticationSettings authenticationSettings);
        public Task RefreshXsrfTokenAsync<TPage>(TPage targetPage);
    }
}