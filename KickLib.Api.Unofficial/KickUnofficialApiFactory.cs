using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial
{
    /// <summary>
    ///     Factory for creating Unofficial Api instances
    /// </summary>
    public class KickUnofficialApiFactory : IKickUnofficialApiFactory
    {
        private readonly SessionManager _sessionManager;
        private readonly BrowserManager _browserManager;
        private readonly ILoggerFactory _loggerFactory;

        public KickUnofficialApiFactory(
            SessionManager sessionManager, 
            BrowserManager browserManager, 
            ILoggerFactory loggerFactory)
        {
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _browserManager = browserManager ?? throw new ArgumentNullException(nameof(browserManager));
            _loggerFactory = loggerFactory;
        }

        public IUnofficialSessionKickApi CreateSessionInstance(
            string sessionId = null, 
            BrowserSettings browserSettings = null, 
            ILogger logger = null)
        {
            logger ??= _loggerFactory?.CreateLogger<SessionKickUnofficialApi>();
            return new SessionKickUnofficialApi(_sessionManager, _browserManager, sessionId, browserSettings, logger);
        }

        public IUnofficialKickApi CreateInstance(
            IApiCaller client = null, 
            ILogger logger = null)
        {
            return new KickUnofficialApi(client, logger);
        }
    }
}
