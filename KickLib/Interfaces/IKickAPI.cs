using KickLib.Api;

namespace KickLib.Interfaces;

public interface IKickApi
{
    Channels Channels { get; }
    Livestream Livestream { get; }
    Users Users { get; }
}