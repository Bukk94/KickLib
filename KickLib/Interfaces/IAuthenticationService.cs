namespace KickLib.Interfaces;

public interface IAuthenticationService
{
    string BearerToken { get; }
    public bool IsAuthenticated { get; }

    public Task AuthenticateAsync(string username, string password);
}