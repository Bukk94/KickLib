using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace KickLib.Extensions;

internal static class LoggerExtensions
{
    internal static ILogger<TType> GetLogger<TType>(this ILoggerFactory? loggerFactory)
    {
        return loggerFactory?.CreateLogger<TType>() ?? NullLogger<TType>.Instance;
    }
}