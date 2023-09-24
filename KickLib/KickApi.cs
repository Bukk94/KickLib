using KickLib.Api;
using KickLib.Clients;
using KickLib.Interfaces;
using Microsoft.Extensions.Logging;

namespace KickLib;

public class KickApi : IKickApi
{
    private readonly IApiCaller _client;
    
    public Clips Clips { get; }
    public Channels Channels { get; }
    public Emotes Emotes { get; }
    public Livestream Livestream { get; }
    public Messages Messages { get; }
    public Users Users { get; }

    public KickApi(IApiCaller client = null, ILogger logger = null)
    {
        client ??= new BrowserClient(new AuthenticationService());
        
        Clips = new Clips(client, logger);
        Channels = new Channels(client, logger);
        Emotes = new Emotes(client, logger);
        Livestream = new Livestream(client, logger);
        Messages = new Messages(client, logger);
        Users = new Users(client, logger);

        _client = client;
    }

    public Task AuthenticateAsync(string username, string password, string totp)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentNullException(nameof(username));
        }
        
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }
        
        return _client.AuthenticateAsync(username, password, totp);
    }
}