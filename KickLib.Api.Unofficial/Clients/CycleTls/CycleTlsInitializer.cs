using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Clients.CycleTls
{
    public static class CycleTlsInitializer
    {
        private static SpoofSettings _settings;
    
        public static CycleTLSClient Client { get; private set; }

        public static void Initialize(SpoofSettings settings, ILogger logger)
        {
            _settings = settings ?? SpoofSettings.Empty;

            if (Client is null)
            {
                Client = new CycleTLSClient(logger);
                Client.InitializeServerAndClient();
            }
        } 
    
        public static CycleTlsRequestOptions GetOptions(string url)
        {
            return new CycleTlsRequestOptions
            {
                Ja3 = _settings.Ja3,
                Url = url,
                UserAgent = UserAgentRandomizer.GetRandomUserAgent(),
                Headers = new Dictionary<string, string>
                {
                    { "Accept", "application/json" },
                    { "Accept-Encoding", "gzip, deflate, br, zstd" },
                    { "Accept-Language", "en,cs;q=0.9" },
                    { "Content-Type", "application/json" }
                },
                DisableRedirect = false
            };
        }
    }
}