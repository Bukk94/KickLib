using KickLib.Api.Unofficial.Models;
using System.Collections.Concurrent;

namespace KickLib.Api.Unofficial.Core
{
    /// <summary>
    /// Manages multiple user sessions and their authentication states
    /// </summary>
    public class SessionManager
    {
        private readonly ConcurrentDictionary<string, UserSession> _sessions = new();
        private readonly BrowserManager _browserManager;

        public SessionManager(BrowserManager browserManager)
        {
            _browserManager = browserManager;
        }

        public string CreateSession(string userId = null)
        {
            var sessionId = userId ?? Guid.NewGuid().ToString();
            var session = new UserSession
            {
                SessionId = sessionId,
                CreatedAt = DateTime.UtcNow,
                LastAccessedAt = DateTime.UtcNow
            };

            _sessions.TryAdd(sessionId, session);
            return sessionId;
        }

        public UserSession GetSession(string sessionId)
        {
            if (_sessions.TryGetValue(sessionId, out var session))
            {
                session.LastAccessedAt = DateTime.UtcNow;
                return session;
            }
            return null;
        }

        public bool UpdateSessionAuth(string sessionId, string bearerToken, string xsrfToken)
        {
            if (_sessions.TryGetValue(sessionId, out var session))
            {
                session.BearerToken = bearerToken;
                session.XsrfToken = xsrfToken;
                session.IsAuthenticated = !string.IsNullOrEmpty(bearerToken);
                session.LastAccessedAt = DateTime.UtcNow;
                return true;
            }
            return false;
        }

        public void RemoveSession(string sessionId)
        {
            if (_sessions.TryRemove(sessionId, out var session))
            {
                // Clean up browser session
                _browserManager.RemoveSession(sessionId);
            }
        }

        public void CleanupExpiredSessions(TimeSpan expiry)
        {
            var expiredSessions = _sessions.Where(kvp => 
                DateTime.UtcNow - kvp.Value.LastAccessedAt > expiry)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var sessionId in expiredSessions)
            {
                RemoveSession(sessionId);
            }
        }

        public IEnumerable<string> GetAllSessionIds()
        {
            return _sessions.Keys.ToList();
        }
    }

    public class UserSession
    {
        public string SessionId { get; set; }
        public string BearerToken { get; set; }
        public string XsrfToken { get; set; }
        public bool IsAuthenticated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessedAt { get; set; }
        public AuthenticationSettings AuthenticationSettings { get; set; }
    }
}
