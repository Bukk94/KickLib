using KickLib.Api;
using KickLib.Models;

namespace KickLib.Interfaces;

public interface IKickApi
{
    Clips Clips { get; }
    Channels Channels { get; }
    Emotes Emotes { get; }
    Livestream Livestream { get; }
    Messages Messages { get; }
    Users Users { get; }

    Task AuthenticateAsync(AuthenticationSettings authenticationSettings);
}