using KickLib.Api;

namespace KickLib.Interfaces;

public interface IKickApi
{
    Clips Clips { get; }
    Channels Channels { get; }
    Emotes Emotes { get; }
    Livestream Livestream { get; }
    Messages Messages { get; }
    Users Users { get; }

    Task AuthenticateAsync(string username, string password, string totp);
}