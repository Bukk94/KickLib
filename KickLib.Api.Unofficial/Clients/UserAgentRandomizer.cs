namespace KickLib.Api.Unofficial.Clients;

internal static class UserAgentRandomizer
{
    private static readonly Random Random = new();
    private static readonly object SyncLock = new();

    internal static string GetRandomUserAgent()
    {
        string userAgent;
        var browserType = new[] { "chrome", "firefox", };
        var uaTemplate = new Dictionary<string, string>
        {
            { "chrome", "Mozilla/5.0 ({0}) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/{1} Safari/537.36" },
            { "firefox", "Mozilla/5.0 ({0}; rv:{1}.0) Gecko/20100101 Firefox/{1}.0" },
        };
            
        var os = new[]
        {
            "Windows NT 10.0; Win64; x64", 
            "X11; Linux x86_64", 
            "Macintosh; Intel Mac OS X 12_4"
        };

        lock (SyncLock)
        {
            var osSystem = os[Random.Next(os.Length)];
            var version = Random.Next(93, 104);
            var minor = 0;
            var patch = Random.Next(4950, 5162);
            var build = Random.Next(80, 212);
            var randomBrowser = browserType[Random.Next(browserType.Length)];
            var browserTemplate = uaTemplate[randomBrowser];
            var finalVersion = version.ToString();
            if (randomBrowser == "chrome")
            {
                finalVersion = string.Format("{0}.{1}.{2}.{3}", version, minor, patch, build);
            }

            userAgent = string.Format(browserTemplate, osSystem, finalVersion);
        }

        return userAgent;
    }
}