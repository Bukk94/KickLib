using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Interfaces
{
    /// <summary>
    /// Factory for creating OptimizedKickUnofficialApi instances
    /// </summary>
    public interface IOptimizedKickUnofficialApiFactory
    {
        /// <summary>
        /// Create a new API instance with a specific session ID
        /// </summary>
        /// <param name="sessionId">Session ID (optional, will generate if not provided)</param>
        /// <param name="browserSettings">Browser settings (optional)</param>
        /// <param name="logger">Logger instance (optional)</param>
        /// <returns>New API instance</returns>
        OptimizedKickUnofficialApi CreateInstance(string sessionId = null, BrowserSettings browserSettings = null, ILogger logger = null);
    }
}
