using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Interfaces
{
    /// <summary>
    ///     Factory for creating Unofficial API instances.
    /// </summary>
    public interface IKickUnofficialApiFactory
    {
        /// <summary>
        ///     Create a new API instance with a specific session ID.
        /// </summary>
        /// <param name="sessionId">Session ID (optional, will generate if not provided)</param>
        /// <param name="browserSettings">Browser settings (optional)</param>
        /// <param name="logger">Logger instance (optional)</param>
        /// <returns>New API instance</returns>
        IUnofficialSessionKickApi CreateSessionInstance(string sessionId = null, BrowserSettings browserSettings = null, ILogger logger = null);
        
        /// <summary>
        ///     Create standard API instance.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        IUnofficialKickApi CreateInstance(IApiCaller client = null, ILogger logger = null);
    }
}
