using KickLib.Api;
using KickLib.Models;

namespace KickLib.Interfaces;

public interface IKickApi
{
    Categories Categories { get; }
    Clips Clips { get; }
    Channels Channels { get; }
    Emotes Emotes { get; }
    Livestream Livestream { get; }
    Messages Messages { get; }
    Users Users { get; }
    Videos Videos { get; }

    Task AuthenticateAsync(AuthenticationSettings authenticationSettings);
}