using KickLib.Api;
using KickLib.Clients;
using KickLib.Interfaces;

namespace KickLib;

public class KickApi : IKickApi
{
    public Channels Channels { get; }
    public Livestream Livestream { get; }
    public Users Users { get; }

    public KickApi(IApiCaller client = null)
    {
        client ??= new BrowserClient();
        
        Channels = new Channels(client);
        Livestream = new Livestream(client);
        Users = new Users(client);
    }
}