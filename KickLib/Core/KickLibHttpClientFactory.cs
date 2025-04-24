namespace KickLib.Core;

internal class KickLibHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string? name = null)
    {
        return new HttpClient();
    }
}