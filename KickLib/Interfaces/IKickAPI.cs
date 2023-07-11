using KickLib.Api;

namespace KickLib.Interfaces;

public interface IKickApi
{
    Clips Clips { get; }
    Channels Channels { get; }
    Livestream Livestream { get; }
    Users Users { get; }
}