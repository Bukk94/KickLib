using KickLib.Api.Unofficial.Clients;
using KickLib.Api.Unofficial.Clients.Puppeteer;
using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial
{
    /// <summary>
    ///     Multi-user optimized KickUnofficialApi with shared browser instance
    /// </summary>
    public class SessionKickUnofficialApi : KickUnofficialApi, IUnofficialSessionKickApi, IDisposable
    {
        private readonly string _sessionId;
        private readonly SessionManager _sessionManager;
        private readonly ILogger _logger;
        private bool _disposed;

        /// <summary>
        ///     Session ID for this API instance
        /// </summary>
        public string SessionId => _sessionId;

        /// <summary>
        ///     Indicates if this session is authenticated
        /// </summary>
        public bool IsAuthenticated => _sessionManager.GetSession(_sessionId)?.IsAuthenticated ?? false;

        public SessionKickUnofficialApi(
            SessionManager sessionManager, 
            BrowserManager browserManager, 
            string sessionId = null, 
            BrowserSettings browserSettings = null, 
            ILogger logger = null)
        : base(GetClient(sessionManager, browserManager, sessionId, browserSettings, logger, out var currentSessionId), logger)
        {
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _sessionId = currentSessionId;
            _logger = logger;
            _logger?.LogInformation("OptimizedKickUnofficialApi initialized for session {SessionId}", _sessionId);
        }

        private static SessionBrowserClient GetClient(
            SessionManager sessionManager, 
            BrowserManager browserManager, 
            string sessionId,
            BrowserSettings browserSettings,
            ILogger logger,
            out string currentSessionId)
        {
            if (sessionManager == null)
            {
                throw new ArgumentNullException(nameof(sessionManager));
            }

            // Create or use existing session
            currentSessionId = sessionId ?? sessionManager.CreateSession();
            
            // Ensure session exists
            if (sessionManager.GetSession(currentSessionId) == null)
            {
                sessionManager.CreateSession(currentSessionId);
            }

            browserSettings ??= BrowserSettings.Empty;
            
            // Create optimized authentication service for this session
            var authService = new OptimizedPuppeteerAuthenticationService(currentSessionId, browserSettings, browserManager, sessionManager, logger);

            // Create optimized client for this session
            return new SessionBrowserClient(authService, currentSessionId, browserManager, sessionManager, logger);
        }

        /// <summary>
        /// Create a new API instance for a specific user/session
        /// Note: This method is deprecated. Use dependency injection instead.
        /// </summary>
        /// <param name="userId">Unique identifier for the user</param>
        /// <param name="browserSettings">Browser configuration (optional)</param>
        /// <param name="logger">Logger instance (optional)</param>
        /// <returns>New API instance for the specified user</returns>
        [Obsolete("Use dependency injection instead")]
        public static SessionKickUnofficialApi CreateForUser(string userId, BrowserSettings browserSettings = null, ILogger logger = null)
        {
            var browserManager = new BrowserManager(browserSettings, logger as ILogger<BrowserManager>);
            var sessionManager = new SessionManager(browserManager);
            return new SessionKickUnofficialApi(sessionManager, browserManager, userId, browserSettings, logger);
        }

        /// <summary>
        ///     Get all active session IDs
        /// </summary>
        /// <returns>Collection of active session IDs</returns>
        public IEnumerable<string> GetActiveSessions()
        {
            return _sessionManager.GetAllSessionIds();
        }

        /// <summary>
        ///     Clean up expired sessions.
        /// </summary>
        /// <param name="expiry">Session expiry time (default: 1 hour)</param>
        public void CleanupExpiredSessions(TimeSpan? expiry = null)
        {
            _sessionManager.CleanupExpiredSessions(expiry ?? TimeSpan.FromHours(1));
        }

        /// <summary>
        ///     Remove a specific session.
        /// </summary>
        /// <param name="sessionId">Session ID to remove</param>
        public void RemoveSession(string sessionId)
        {
            _sessionManager.RemoveSession(sessionId);
        }

        /// <summary>
        ///     Get session information.
        /// </summary>
        /// <returns>Current session information</returns>
        public UserSession GetSessionInfo()
        {
            return _sessionManager.GetSession(_sessionId);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _logger?.LogInformation("Disposing OptimizedKickUnofficialApi for session {SessionId}", _sessionId);
            
            // Remove this session
            _sessionManager.RemoveSession(_sessionId);
            
            _disposed = true;
        }
    }
}
