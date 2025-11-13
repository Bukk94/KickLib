using KickLib.Api.Unofficial;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using KickLib.Api.Unofficial.Models.Response.v1.Emotes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KickLib.Examples;

/// <summary>
///     Unofficial Kick API (IUnofficialKickApi) Usage Examples
/// 
///     This example demonstrates how to use the unofficial/undocumented Kick API.
///     The unofficial API provides access to features not yet available in the official API,
///     such as getting videos/clips, getting detailed channel data, and more.
/// 
///     WARNING: This uses undocumented endpoints that may change without notice.
///     Use at your own risk. For production applications, prefer the official API when possible.
///
///     NOTE: IDs and Tokens are NOT interchangeable between the official and unofficial APIs!
/// </summary>
public static class UnofficialApiExample
{
    /// <summary>
    /// Example 1: Basic Setup with Dependency Injection (Recommended)
    /// </summary>
    public static void Example1_SetupWithDependencyInjection()
    {
        // Setup this in your Dependency Injection configuration
        var serviceProvider = new ServiceCollection()
            .AddUnofficialKickLib()
            .WithSessionPuppeteerClient() // Uses Puppeteer for browser automation
            // or you can use .WithTlsClient() or .WithSessionPuppeteerClient()
            // you cannot combine the client types in the same DI setup
            // 
            // Optionally, add logging (you can use `Microsoft.Extensions.Logging.Console` package) with simple Console logger
            .AddLogging(builder => builder.AddConsole())
            .BuildServiceProvider();

        // Get the API factory from DI or via constructor injection
        var apiFactory = serviceProvider.GetRequiredService<IKickUnofficialApiFactory>();
        
        // Create standard API instance
        var api = apiFactory.CreateInstance();
        
        Console.WriteLine($"Unofficial API setup complete! Type: {api.GetType().Name}");
        
        // Create standard API instance
        var sessionApi = apiFactory.CreateSessionInstance("my_session_id");
    }

    /// <summary>
    /// Example 2: Manual Setup without Dependency Injection
    /// </summary>
    public static void Example2_ManualSetup()
    {
        // Create logger (optional but recommended) - Requires `Microsoft.Extensions.Logging.Console` package
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger("UnofficialApi");
        
        // Create API instance with default browser client
        IUnofficialKickApi api = new KickUnofficialApi(logger: logger);
        
        Console.WriteLine($"Unofficial API instance created!");
    }

    /// <summary>
    /// Example 3: Authentication with Username and Password
    /// </summary>
    public static async Task Example3_AuthenticateWithCredentials(IUnofficialKickApi api)
    {
        // Create authentication settings
        var authSettings = new AuthenticationSettings("your_username", "your_password")
        {
            // For all Auth endpoints, 2FA is required
            TwoFactorAuthCode = "YOUR_2FA_SECRET",
            UseOtp = true
        };
        
        try
        {
            await api.AuthenticateAsync(authSettings);
            Console.WriteLine("✓ Authentication successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Authentication failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Example 4: Authentication with Bearer Token Override
    /// </summary>
    public static async Task Example4_AuthenticateWithToken(IUnofficialKickApi api)
    {
        // This case is useful when Kick blocks access to /login endpoint
        
        // Use a pre-existing bearer token (skip login process)
        var authSettings = AuthenticationSettings.WithTokenOverride("Bearer YOUR_TOKEN_HERE");
        
        await api.AuthenticateAsync(authSettings);
        Console.WriteLine("✓ Token applied successfully!");
    }

    /// <summary>
    /// Example 5: Get Channel Information
    /// </summary>
    public static async Task Example5_GetChannelInfo(IUnofficialKickApi api)
    {
        var channelInfo = await api.Channels.GetChannelInfoAsync("kicklib");
        
        if (channelInfo != null)
        {
            Console.WriteLine($"Channel: {channelInfo.User.Username}");
            Console.WriteLine($"ID: {channelInfo.Id}");
            Console.WriteLine($"Followers: {channelInfo.FollowersCount}");
            Console.WriteLine($"Bio: {channelInfo.User.Bio}");
            Console.WriteLine($"Chatroom ID: {channelInfo.Chatroom.Id}");
            Console.WriteLine($"Is Live: {channelInfo.Livestream != null}");
            
            if (channelInfo.Livestream != null)
            {
                Console.WriteLine($"  Stream Title: {channelInfo.Livestream.SessionTitle}");
                Console.WriteLine($"  Viewers: {channelInfo.Livestream.Viewers}");
            }
        }
    }

    /// <summary>
    /// Example 6: Get Channel Messages (Chat History)
    /// </summary>
    public static async Task Example6_GetChannelMessages(IUnofficialKickApi api)
    {
        var chatroomId = 4704335; // Get this from channel info
        
        // Get first page of messages
        var messages = await api.Channels.GetChannelMessagesAsync(chatroomId);
        
        if (messages != null)
        {
            Console.WriteLine($"Retrieved {messages.Messages.Count} messages:");
            foreach (var message in messages.Messages.Take(5))
            {
                Console.WriteLine($"  [{message.CreatedAt}] {message.Sender.Username}: {message.Content}");
            }
            
            // Get next page using cursor
            if (!string.IsNullOrEmpty(messages.Cursor))
            {
                var nextPage = await api.Channels.GetChannelMessagesAsync(chatroomId, messages.Cursor);
                Console.WriteLine($"\nNext page has {nextPage?.Messages.Count} messages");
            }
        }
    }

    /// <summary>
    /// Example 7: Send Chat Message (Requires Authentication)
    /// </summary>
    public static async Task Example7_SendMessage(IUnofficialKickApi api)
    {
        var chatroomId = 4704335;
        
        try
        {
            // Must be authenticated first (can be done once globally)
            var authSettings = AuthenticationSettings.WithTokenOverride("Bearer YOUR_TOKEN_HERE");
            await api.AuthenticateAsync(authSettings);
            
            // Send message
            await api.Messages.SendMessageAsync(chatroomId, "Hello from KickLib! 👋");
            Console.WriteLine("✓ Message sent successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Failed to send message: {ex.Message}");
        }
    }

    /// <summary>
    /// Example 8: Delete Chat Message (Requires Authentication & Permissions)
    /// </summary>
    public static async Task Example8_DeleteMessage(IUnofficialKickApi api)
    {
        var chatroomId = 4704335;
        var messageId = "a00a1592-be91-4abe-9689-50cc0a87eeac"; // ID of the message
        
        try
        {
            // Must be authenticated first (can be done once globally)
            var authSettings = AuthenticationSettings.WithTokenOverride("Bearer YOUR_TOKEN_HERE");
            await api.AuthenticateAsync(authSettings);
            
            var success = await api.Messages.DeleteMessageAsync(chatroomId, messageId);
            
            if (success)
            {
                Console.WriteLine("✓ Message deleted successfully!");
            }
            else
            {
                Console.WriteLine("✗ Failed to delete message");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error deleting message: {ex.Message}");
        }
    }

    /// <summary>
    /// Example 9: Get User Information
    /// </summary>
    public static async Task Example9_GetUserInfo(IUnofficialKickApi api)
    {
        var user = await api.Users.GetUserAsync("kicklib");
        
        if (user != null)
        {
            Console.WriteLine($"User: {user.Username}");
            Console.WriteLine($"ID: {user.Id}");
            Console.WriteLine($"Bio: {user.Bio}");
            Console.WriteLine($"Profile Picture: {user.ProfilePicture}");
        }
    }

    /// <summary>
    /// Example 10: Check if Streamer is Live
    /// </summary>
    public static async Task Example10_CheckIfLive(IUnofficialKickApi api)
    {
        var isLive = await api.Livestream.IsStreamerLiveAsync("kicklib");
        
        if (isLive)
        {
            Console.WriteLine("✓ Streamer is currently live!");
            
            // Get detailed livestream info
            var liveInfo = await api.Livestream.GetLivestreamInfoAsync("kicklib");
            if (liveInfo != null)
            {
                Console.WriteLine($"  Title: {liveInfo.SessionTitle}");
                Console.WriteLine($"  Viewers: {liveInfo.Viewers}");
                Console.WriteLine($"  Category: {liveInfo.Categories.FirstOrDefault()?.Name}");
            }
        }
        else
        {
            Console.WriteLine("✗ Streamer is offline");
        }
    }

    /// <summary>
    /// Example 11: Get Channel Emotes
    /// </summary>
    public static async Task Example11_GetEmotes(IUnofficialKickApi api)
    {
        var emotes = await api.Emotes.GetChannelEmotesAsync("kicklib");
        
        if (emotes != null)
        {
            Console.WriteLine($"Found {emotes.Count} emotes:");
            foreach (var emote in emotes)
            {
                if (emote is KickEmotesResponse kickEmote)
                {
                    Console.WriteLine($"  - {kickEmote.Name} (ID: {kickEmote.Id})");
                }
                else if (emote is UserEmotesResponse userEmote)
                {
                    Console.WriteLine($"  - {userEmote.Slug}(ID: {userEmote.Id})"); 
                }
                else
                {
                    Console.WriteLine("  - Unknown emote type");
                }
            }
        }
    }

    /// <summary>
    /// Example 12: Get Clips
    /// </summary>
    public static async Task Example12_GetClips(IUnofficialKickApi api)
    {
        // Get all clips (paginated)
        var allClips = await api.Clips.GetClipsAsync();
        if (allClips != null && allClips.Clips.Count > 0)
        {
            Console.WriteLine($"Found {allClips.Clips.Count} clips");
        }
        
        // Get specific clip
        var clipId = 8240;
        var clip = await api.Clips.GetClipAsync(clipId);
        if (clip != null)
        {
            Console.WriteLine($"\nClip: {clip.Title}");
            Console.WriteLine($"Creator: {clip.Creator.Username}");
            Console.WriteLine($"Likes: {clip.LikesCount}");
            Console.WriteLine($"Views: {clip.ViewCount}");
        }
    }

    /// <summary>
    /// Example 13: Get Last Subscriber
    /// </summary>
    public static async Task Example13_GetLastSubscriber(IUnofficialKickApi api)
    {
        // Must be authenticated first (can be done once globally)
        var authSettings = AuthenticationSettings.WithTokenOverride("Bearer YOUR_TOKEN_HERE");
        await api.AuthenticateAsync(authSettings);
        
        var lastSub = await api.Channels.GetLastSubscriberAsync("your_channel");
        
        if (lastSub != null)
        {
            Console.WriteLine($"Latest subscriber: {lastSub.LastSubscriber}");
            Console.WriteLine($"Total count: {lastSub.Count}");
        }
    }

    /// <summary>
    /// Example 14: Get Videos
    /// </summary>
    public static async Task Example14_GetVideos(IUnofficialKickApi api)
    {
        var videoGuid = Guid.NewGuid(); // Replace with actual video GUID
        var video = await api.Videos.GetVideoAsync(videoGuid);
        
        if (video != null)
        {
            Console.WriteLine($"Video: {video.Livestream.SessionTitle}");
            Console.WriteLine($"Created: {video.CreatedAt}");
            Console.WriteLine($"Views: {video.Views}");
        }
    }

    /// <summary>
    /// Example 15: Get Categories
    /// </summary>
    public static async Task Example15_GetCategories(IUnofficialKickApi api)
    {
        var categories = await api.Categories.GetCategoriesAsync();
        
        if (categories != null && categories.Any())
        {
            Console.WriteLine("Available categories:");
            foreach (var category in categories.Take(10))
            {
                Console.WriteLine($"  - {category.Name} (Slug: {category.Slug})");
            }
        }
    }

    /// <summary>
    /// Complete workflow example combining authentication and multiple features
    /// </summary>
    public static async Task Example16_CompleteWorkflow()
    {
        // 1. Setup - Requires `Microsoft.Extensions.Logging.Console` package
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger("UnofficialApi");
        IUnofficialKickApi api = new KickUnofficialApi(logger: logger);
        
        // 2. Authenticate (optional - only needed for write operations)
        var authSettings = AuthenticationSettings.WithTokenOverride("YOUR_TOKEN");
        await api.AuthenticateAsync(authSettings);
        Console.WriteLine("✓ Authenticated\n");
        
        // 3. Get channel info
        var channelInfo = await api.Channels.GetChannelInfoAsync("kicklib");
        if (channelInfo != null)
        {
            Console.WriteLine($"✓ Channel: {channelInfo.User.Username}");
            Console.WriteLine($"  Followers: {channelInfo.FollowersCount}");
            Console.WriteLine($"  Chatroom ID: {channelInfo.Chatroom.Id}\n");
            
            // 4. Check if live
            var isLive = await api.Livestream.IsStreamerLiveAsync(channelInfo.User.Username);
            Console.WriteLine($"✓ Is Live: {isLive}\n");
            
            // 5. Get chat history
            var messages = await api.Channels.GetChannelMessagesAsync(channelInfo.Chatroom.Id);
            Console.WriteLine($"✓ Retrieved {messages?.Messages.Count ?? 0} chat messages\n");
            
            // 6. Get emotes
            var emotes = await api.Emotes.GetChannelEmotesAsync(channelInfo.User.Username);
            Console.WriteLine($"✓ Found {emotes?.Count ?? 0} emotes\n");
        }
    }

    /// <summary>
    /// Example 17: Error Handling Best Practices
    /// </summary>
    public static async Task Example17_ErrorHandling(IUnofficialKickApi api)
    {
        try
        {
            var channelInfo = await api.Channels.GetChannelInfoAsync("nonexistent_channel_12345");
            
            if (channelInfo == null)
            {
                Console.WriteLine("Channel not found");
            }
            else
            {
                Console.WriteLine($"Channel found: {channelInfo.User.Username}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
            
            // Log the full exception for debugging
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }
}
