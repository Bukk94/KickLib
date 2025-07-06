using KickLib.Api.Unofficial.Core;

namespace KickLib.Api.Unofficial.Interfaces
{
    public interface IUnofficialSessionKickApi : IUnofficialKickApi
    {
        string SessionId { get; }
        bool IsAuthenticated { get; }

        IEnumerable<string> GetActiveSessions();
        UserSession GetSessionInfo();
        void RemoveSession(string sessionId);
        void CleanupExpiredSessions(TimeSpan? expiry = null);
    }
}