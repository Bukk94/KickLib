using KickLib;
using KickLib.Auth;
using KickLib.Client;
using KickLib.Client.Interfaces;
using KickLib.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KickLib.Examples;

/// <summary>
///     This is a development test example for KickLib.
///     It demonstrates how to obtain OAuth tokens on local machine and use them to send messages as a bot.
///     It also shows how to connect to a chat room and listen for incoming messages.
///
///     This call is BLOCKING and will keep running until manually stopped (it listens to chat messages).
///
///     REQUIREMENTS:
///     * You must have a registered Kick application
///     * Client ID and Client Secret from the application
///     * Redirect URL must match the one registered in the application
///
///     Usage:
///     await DevelopmentTestAsync.TestAsync();
/// </summary>
public static class DevelopmentTest
{
    // Get these from your application: https://kick.com/settings/developer
    private const string ClientId = "XXXXXXXXXXXXXXXXXX";
    private const string ClientSecret = "YYYYYYYYYYYYYYYYYYYYY";
    private const int ChatroomId = 4704335; // Replace with actual chatroom ID to connect to
    
    public static async Task TestAsync(bool useLogger = false)
    {
        if (!File.Exists("refreshToken.txt"))
        {
            File.WriteAllText("refreshToken.txt", "");
        }

        if (!File.Exists("accessToken.txt"))
        {
            File.WriteAllText("accessToken.txt", "");
        }
            
        // API - To Send messages
        var loggerFactory = useLogger ? LoggerFactory.Create(builder => builder.AddConsole()) : null;
        var settings = new ApiSettings
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            AccessToken = File.ReadAllText("accessToken.txt"),
            RefreshToken = File.ReadAllText("refreshToken.txt"),
        };
        
        var api = KickApi.Create(settings, loggerFactory);
        api.ApiSettings.AccessTokenChanged += (_, args) =>
        {
            Console.WriteLine($"Access token updated: {args.NewToken}");
            File.WriteAllText("accessToken.txt", args.NewToken);
        };
        
        api.ApiSettings.RefreshTokenChanged += (_, args) =>
        {
            Console.WriteLine($"Refresh token updated: {args.NewToken}");
            File.WriteAllText("refreshToken.txt", args.NewToken);
        };

        var tokens = await LocalOAuth.PerformKickLoginAsync([KickScopes.ChatWrite]);
        api.ApiSettings.AccessToken = tokens.AccessToken;
        api.ApiSettings.RefreshToken = tokens.RefreshToken;
        
        // SEND MESSAGE EXAMPLE
        await api.Chat.SendMessageAsBotAsync("Hello from KickLib");
        
        // CLIENT - reading
        IKickClient client = new KickClient();
        client.OnMessage += (sender, e) =>
        {
            var message = e.Data;
            Console.WriteLine($"[{message.CreatedAt}] {message.Sender.Username}: {message.Content}");
        };
        
        await client.ListenToChatRoomAsync(ChatroomId);
        await client.ConnectAsync();

        while (true)
        {
            // Infinite waiting loop to keep the application running
            await Task.Delay(300);
        }
    }
}