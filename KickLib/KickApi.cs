using KickLib.Api;
using KickLib.Clients;
using KickLib.Interfaces;

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

    public KickApi(IApiCaller client = null)
    {
        client ??= new BrowserClient(new AuthenticationService());
        
        Clips = new Clips(client);
        Channels = new Channels(client);
        Emotes = new Emotes(client);
        Livestream = new Livestream(client);
        Messages = new Messages(client);
        Users = new Users(client);

        _client = client;
    }

    public Task AuthenticateAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentNullException(nameof(username));
        }
        
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }
        
        return _client.AuthenticateAsync(username, password);
    }
}