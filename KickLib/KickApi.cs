using KickLib.Api;
using KickLib.Clients;
using KickLib.Interfaces;

namespace KickLib;

public class KickApi : IKickApi
{
    public Clips Clips { get; }
    public Channels Channels { get; }
    public Livestream Livestream { get; }
    public Users Users { get; }

    public KickApi(IApiCaller client = null)
    {
        client ??= new BrowserClient();
        
        Clips = new Clips(client);
        Channels = new Channels(client);
        Livestream = new Livestream(client);
        Users = new Users(client);
    }
}