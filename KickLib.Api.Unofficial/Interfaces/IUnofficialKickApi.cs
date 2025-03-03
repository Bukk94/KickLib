using KickLib.Api.Unofficial.Api;
using KickLib.Api.Unofficial.Models;

namespace KickLib.Api.Unofficial.Interfaces
{
    public interface IUnofficialKickApi
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
}