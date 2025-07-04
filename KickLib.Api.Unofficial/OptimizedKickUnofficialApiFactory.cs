using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial
{
    /// <summary>
    /// Factory for creating OptimizedKickUnofficialApi instances
    /// </summary>
    public class OptimizedKickUnofficialApiFactory : IOptimizedKickUnofficialApiFactory
    {
        private readonly SessionManager _sessionManager;
        private readonly BrowserManager _browserManager;
        private readonly ILoggerFactory _loggerFactory;

        public OptimizedKickUnofficialApiFactory(
            SessionManager sessionManager, 
            BrowserManager browserManager, 
            ILoggerFactory loggerFactory)
        {
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _browserManager = browserManager ?? throw new ArgumentNullException(nameof(browserManager));
            _loggerFactory = loggerFactory;
        }

        public OptimizedKickUnofficialApi CreateInstance(string sessionId = null, BrowserSettings browserSettings = null, ILogger logger = null)
        {
            logger ??= _loggerFactory?.CreateLogger<OptimizedKickUnofficialApi>();
            return new OptimizedKickUnofficialApi(_sessionManager, _browserManager, sessionId, browserSettings, logger);
        }
    }
}
