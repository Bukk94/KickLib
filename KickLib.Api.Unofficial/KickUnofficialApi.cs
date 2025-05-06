using KickLib.Api.Unofficial.Api;
using KickLib.Api.Unofficial.Clients;
using KickLib.Api.Unofficial.Clients.Puppeteer;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial
{
    public class KickUnofficialApi : IUnofficialKickApi
    {
        private readonly IApiCaller _client;
    
        public Categories Categories { get; }
        public Clips Clips { get; }
        public Channels Channels { get; }
        public Emotes Emotes { get; }
        public Livestream Livestream { get; }
        public Messages Messages { get; }
        public Users Users { get; }
        public Videos Videos { get; }

        public KickUnofficialApi(IApiCaller client = null, ILogger logger = null)
        {
            var browserSettings = BrowserSettings.Empty;
            client ??= new BrowserClient(new PuppeteerAuthenticationService(browserSettings, logger), browserSettings, logger);
        
            Categories = new Categories(client, logger);
            Clips = new Clips(client, logger);
            Channels = new Channels(client, logger);
            Emotes = new Emotes(client, logger);
            Livestream = new Livestream(client, logger);
            Messages = new Messages(client, logger);
            Users = new Users(client, logger);
            Videos = new Videos(client, logger);

            _client = client;
        }

        public Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
        {
            if (authenticationSettings is null)
            {
                throw new ArgumentNullException(nameof(authenticationSettings));
            }
        
            return _client.AuthenticateAsync(authenticationSettings);
        }
    }
}