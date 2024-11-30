using Microsoft.Extensions.Logging;

namespace KickLib.Clients.CycleTls;

public class CycleTLSClient
{
    private readonly ILogger _logger;
    
    public CycleTLSClient(ILogger logger)
    {
        _logger = logger ?? new LoggerFactory().CreateLogger(nameof(CycleTLSClient));
    }
    
    public void InitializeServerAndClient(int port = 9119)
    {
        // TODO: Implement server initialization
        _logger.LogError("Cycle TLS client is missing implementation!");
    }

    public async Task<CycleTlsResponse> SendAsync(CycleTlsRequestOptions cycleTLSRequestOptions)
    {
        // TODO: Implement CycleTLS request sending
        throw new NotSupportedException("Sending via CycleTLS client is not yet implemented!");
    }
}