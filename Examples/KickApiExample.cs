using KickLib;
using KickLib.Auth;
using KickLib.Core;
using KickLib.Models.v1.EventSubscriptions;
using KickLib.Models.v1.Moderation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KickLib.Examples;

/// <summary>
///     Official Kick API (IKickApi) Usage Examples
/// 
///     This example demonstrates how to use the official Kick API with OAuth authentication.
///     The official API provides access to channels, users, categories, livestreams, chat, moderation, and webhook subscriptions.
/// 
///     Prerequisites:
///     - Register your application at https://kick.com/settings/developer
///     - Obtain Client ID and Client Secret
///     - Set up a redirect URI for OAuth callback
/// </summary>
public static class OfficialApiExample
{
    // Replace these with your actual application credentials
    private const string ClientId = "YOUR_CLIENT_ID";
    private const string ClientSecret = "YOUR_CLIENT_SECRET";
    private const string RedirectUri = "http://localhost:5000/kick/callback";

    /// <summary>
    /// Example 1: Basic Setup with Dependency Injection (Recommended)
    /// </summary>
    public static IKickApi Example1_SetupWithDependencyInjection()
    {
        // Setup this in your Dependency Injection configuration
        var serviceProvider = new ServiceCollection()
            .AddKickLib()
            // Optionally, add logging (you can use `Microsoft.Extensions.Logging.Console` package) with simple Console logger 
            .AddLogging(builder => builder.AddConsole())
            .BuildServiceProvider();

        // Get the API instance from DI or via constructor injection
        var api = serviceProvider.GetRequiredService<IKickApi>();
        
        // Configure API settings
        api.ApiSettings.ClientId = ClientId;
        api.ApiSettings.ClientSecret = ClientSecret;
        api.ApiSettings.AccessToken = "YOUR_ACCESS_TOKEN"; // Set after OAuth flow
        api.ApiSettings.RefreshToken = "YOUR_REFRESH_TOKEN";
        
        // Subscribe to token change events to persist tokens
        api.ApiSettings.AccessTokenChanged += (_, args) =>
        {
            Console.WriteLine($"Access token updated: {args.NewToken}");
            // Save to database/file for persistence
        };
        
        api.ApiSettings.RefreshTokenChanged += (_, args) =>
        {
            Console.WriteLine($"Refresh token updated: {args.NewToken}");
            // Save to database/file for persistence
        };
        
        Console.WriteLine("API configured successfully!");
        return api;
    }

    /// <summary>
    /// Example 2: Manual Setup without Dependency Injection
    /// </summary>
    public static IKickApi Example2_ManualSetup()
    {
        // Create logger factory (optional but recommended) - Requires `Microsoft.Extensions.Logging.Console` package
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        
        // Configure API settings
        var settings = new ApiSettings
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            AccessToken = "YOUR_ACCESS_TOKEN",
            RefreshToken = "YOUR_REFRESH_TOKEN"
        };
        
        // Create API instance
        var api = KickApi.Create(settings, loggerFactory);
        
        // Subscribe to token change events to persist tokens
        api.ApiSettings.AccessTokenChanged += (_, args) =>
        {
            Console.WriteLine($"Access token updated: {args.NewToken}");
            // Save to database/file for persistence
        };
        
        api.ApiSettings.RefreshTokenChanged += (_, args) =>
        {
            Console.WriteLine($"Refresh token updated: {args.NewToken}");
            // Save to database/file for persistence
        };
        
        Console.WriteLine($"KickApi instance created successfully!");
        return api;
    }

    /// <summary>
    /// Example 3: OAuth 2.1 Authentication Flow
    /// </summary>
    public static async Task Example3_OAuthFlow()
    {
        var oauthGenerator = new KickOAuthGenerator();
        
        // Step 1: Generate authorization URL
        var authUrl = oauthGenerator.GetAuthorizationUri(
            redirectUri: RedirectUri,
            clientId: ClientId,
            scopes: new List<string>
            {
                KickScopes.UserRead,
                KickScopes.ChannelRead,
                KickScopes.ChannelWrite,
                KickScopes.ChatWrite,
                KickScopes.EventsSubscribe
            },
            verifier: out var codeVerifier,
            state: "optional-state-value" // Optional: Use for CSRF protection
        );
        
        Console.WriteLine($"1. Open this URL in your browser:\n{authUrl}");
        Console.WriteLine("\n2. After authorizing, you'll be redirected to your callback URL with a 'code' parameter.");
        Console.Write("3. Enter the authorization code: ");
        
        var authorizationCode = Console.ReadLine();
        
        // Step 2: Exchange code for tokens
        var tokenResult = await oauthGenerator.ExchangeCodeForTokenAsync(
            code: authorizationCode!,
            clientId: ClientId,
            clientSecret: ClientSecret,
            redirectUrl: RedirectUri,
            state: "optional-state-value",
            verifier: codeVerifier
        );
        
        if (tokenResult.IsSuccess)
        {
            var tokens = tokenResult.Value;
            Console.WriteLine($"\n✓ Authentication successful!");
            Console.WriteLine($"Access Token: {tokens.AccessToken}");
            Console.WriteLine($"Refresh Token: {tokens.RefreshToken}");
            Console.WriteLine($"Expires In: {tokens.ExpiresIn} seconds");
            
            // Save these tokens for future use
        }
        else
        {
            Console.WriteLine($"✗ Authentication failed: {tokenResult.Errors.First().Message}");
        }
    }

    /// <summary>
    /// Example 3.1: OAuth 2.1 Authentication Flow - Getting App Access Token
    /// </summary>
    public async Task Example3_OAuth_App_Flow()
    {
        var gen = new KickOAuthGenerator();
        var result = await gen.GenerateAppAccessTokenAsync(ClientId, ClientSecret);
        if (result.IsSuccess)
        {
            var appToken = result.Value;
            Console.WriteLine($"\n✓ App Access Token generated successfully!");
            Console.WriteLine($"Access Token: {appToken.AccessToken}");
            Console.WriteLine($"Expires In: {appToken.ExpiresIn} seconds");
        }
        else
        {
            Console.WriteLine($"✗ Failed to generate App Access Token: {result.Errors.First().Message}");
        }
    }

    /// <summary>
    /// Example 4: Refresh Access Token
    /// </summary>
    public static async Task Example4_RefreshToken()
    {
        var oauthGenerator = new KickOAuthGenerator();
        
        var refreshResult = await oauthGenerator.RefreshAccessTokenAsync(
            refreshToken: "YOUR_REFRESH_TOKEN",
            clientId: ClientId,
            clientSecret: ClientSecret
        );
        
        if (refreshResult.IsSuccess)
        {
            var tokens = refreshResult.Value;
            Console.WriteLine($"✓ Token refreshed successfully!");
            Console.WriteLine($"New Access Token: {tokens.AccessToken}");
            Console.WriteLine($"New Refresh Token: {tokens.RefreshToken}");
        }
    }

    /// <summary>
    /// Example 5: Fetch Channel Information
    /// </summary>
    public static async Task Example5_GetChannelInfo(IKickApi api)
    {
        var channelResult = await api.Channels.GetChannelAsync("kicklib");
        
        if (channelResult.IsSuccess)
        {
            var channel = channelResult.Value;
            Console.WriteLine($"Channel Slug: {channel.Slug}");
            Console.WriteLine($"Stream Title: {channel.StreamTitle}");
            Console.WriteLine($"Channel Description: {channel.ChannelDescription}");
            Console.WriteLine($"Broadcaster ID: {channel.BroadcasterUserId}");
            Console.WriteLine($"Is Live: {channel.Stream.IsLive}");
            Console.WriteLine($"Viewer Count: {channel.Stream.ViewerCount}");
        }
    }

    /// <summary>
    /// Example 6: Get User Information
    /// </summary>
    public static async Task Example6_GetUserInfo(IKickApi api)
    {
        // Get authenticated user info (requires user:read scope)
        var meResult = await api.Users.GetMeAsync();
        
        if (meResult.IsSuccess)
        {
            var me = meResult.Value;
            Console.WriteLine($"Logged in as: {me.Name}");
            Console.WriteLine($"User ID: {me.UserId}");
            Console.WriteLine($"Email: {me.Email}");
        }
        
        // Get any user by ID
        var userResult = await api.Users.GetUserAsync(123456);
        if (userResult.IsSuccess)
        {
            Console.WriteLine($"User: {userResult.Value.Name}");
        }
    }

    /// <summary>
    /// Example 7: Send Chat Messages
    /// </summary>
    public static async Task Example7_SendChatMessage(IKickApi api)
    {
        // Send message as bot (requires chat:write scope)
        var botMessageResult = await api.Chat.SendMessageAsBotAsync("Hello from my bot!");
        
        if (botMessageResult.IsSuccess)
        {
            Console.WriteLine($"✓ Bot message sent! MessageId: {botMessageResult.Value.MessageId}");
        }
        
        // Send message as authenticated user (requires chat:write scope)
        var chatroomId = 4704335; // Get this from channel info
        var userMessageResult = await api.Chat.SendMessageAsUserAsync(chatroomId, "Hello from user!");
        
        if (userMessageResult.IsSuccess)
        {
            Console.WriteLine($"✓ User message sent! MessageId: {userMessageResult.Value.MessageId}");
        }
    }

    /// <summary>
    /// Example 8: Get Livestream Information
    /// </summary>
    public static async Task Example8_GetLivestreamInfo(IKickApi api)
    {
        // Get all live streams
        var livestreamsResult = await api.Livestreams.GetLivestreamsAsync();
        
        if (livestreamsResult.IsSuccess)
        {
            Console.WriteLine($"Found {livestreamsResult.Value.Count} live streams:");
            foreach (var stream in livestreamsResult.Value)
            {
                Console.WriteLine($"  - {stream.Slug}: {stream.StreamTitle} ({stream.ViewerCount} viewers)");
            }
        }
    }

    /// <summary>
    /// Example 9: Update Channel Information
    /// </summary>
    public static async Task Example9_UpdateChannel(IKickApi api)
    {
        // Requires channel:write scope
        var updateResult = await api.Channels.UpdateChannelAsync(new KickLib.Models.v1.Channels.UpdateChannelRequest
        {
            StreamTitle = "New Stream Title!",
            CategoryId = 28 // Optional: Change category
        });
        
        if (updateResult.IsSuccess)
        {
            Console.WriteLine($"✓ Channel updated successfully!");
        }
    }

    /// <summary>
    /// Example 10: Subscribe to Webhook Events
    /// </summary>
    public static async Task Example10_WebhookSubscriptions(IKickApi api)
    {
        // Subscribe to specific events (requires events:subscribe scope)
        var subscribeResult = await api.EventSubscriptions.SubscribeToEventsAsync(new List<EventType>
        {
            EventType.ChatMessageSent,
            EventType.ChannelFollowed,
            EventType.ChannelSubscriptionNew
        });
        
        if (subscribeResult.IsSuccess)
        {
            Console.WriteLine($"✓ Subscribed to {subscribeResult.Value.Count} events");
        }
        
        // Subscribe to all available events
        var subscribeAllResult = await api.EventSubscriptions.SubscribeToAllEventsAsync();
        if (subscribeAllResult.IsSuccess)
        {
            Console.WriteLine($"✓ Subscribed to all events");
        }
        
        // Get current subscriptions
        var subscriptionsResult = await api.EventSubscriptions.GetEventSubscriptionsAsync();
        if (subscriptionsResult.IsSuccess)
        {
            Console.WriteLine("\nActive subscriptions:");
            foreach (var sub in subscriptionsResult.Value)
            {
                Console.WriteLine($"  - {sub.Event} (ID: {sub.Id})");
            }
        }
        
        // Unsubscribe from specific events
        if (subscriptionsResult.IsSuccess)
        {
            var subscriptionIds = subscriptionsResult.Value.Select(x => x.Id).ToList();
            var unsubscribeResult = await api.EventSubscriptions.UnsubscribeEventsAsync(subscriptionIds);
            
            if (unsubscribeResult.IsSuccess)
            {
                Console.WriteLine($"✓ Unsubscribed from events");
            }
        }
    }

    /// <summary>
    /// Example 11: Browse Categories
    /// </summary>
    public static async Task Example11_GetCategory(IKickApi api)
    {
        // Get a specific category
        var categoryResult = await api.Categories.GetCategoryAsync(28);
        
        if (categoryResult.IsSuccess)
        {
            var category = categoryResult.Value;
            Console.WriteLine($"Category: {category.Name}");
        }
    }

    /// <summary>
    /// Example 12: Moderation Actions
    /// </summary>
    public static async Task Example12_ModerationActions(IKickApi api)
    {
        // Note: All moderation endpoints require moderation:ban scope
        // These operations can only be performed by users with moderation permissions
        
        var broadcasterUserId = 123456; // Your channel's broadcaster user ID
        var userIdToModerate = 789012; // User ID to moderate
        
        // Example 1: Timeout a user using the strong-typed TimeoutDuration
        var timeoutResult = await api.Moderation.TimeoutUserAsync(
            broadcasterUserId: broadcasterUserId,
            userIdToBan: userIdToModerate,
            duration: TimeoutDuration.FiveMinutes, // Type-safe with built-in validation
            reason: "Spamming in chat"
        );
        
        if (timeoutResult.IsSuccess)
        {
            Console.WriteLine($"✓ User timed out for 5 minutes");
        }
        else
        {
            Console.WriteLine($"✗ Failed to timeout user: {timeoutResult.Errors.First().Message}");
        }
        
        // Example 2: Using predefined timeout durations - You should use only ONE call for one user
        await api.Moderation.TimeoutUserAsync(broadcasterUserId, userIdToModerate, TimeoutDuration.OneMinute);
        await api.Moderation.TimeoutUserAsync(broadcasterUserId, userIdToModerate, TimeoutDuration.ThirtyMinutes);
        await api.Moderation.TimeoutUserAsync(broadcasterUserId, userIdToModerate, TimeoutDuration.OneHour);
        await api.Moderation.TimeoutUserAsync(broadcasterUserId, userIdToModerate, TimeoutDuration.OneDay);
        await api.Moderation.TimeoutUserAsync(broadcasterUserId, userIdToModerate, TimeoutDuration.Max); // 7 days
        
        // Example 3: Create custom timeout duration from TimeSpan
        var customDuration = TimeoutDuration.FromTimeSpan(TimeSpan.FromHours(2.5)); // 2.5 hours
        await api.Moderation.TimeoutUserAsync(broadcasterUserId, userIdToModerate, customDuration);
        
        // Example 4: Access Min/Max properties
        Console.WriteLine($"Min timeout: {TimeoutDuration.Min}"); // 1 minute
        Console.WriteLine($"Max timeout: {TimeoutDuration.Max}"); // 10080 minutes (7 days)
        
        // Example 5: Create from explicit minutes with validation
        try
        {
            var validTimeout = new TimeoutDuration(120); // 2 hours - valid
            await api.Moderation.TimeoutUserAsync(broadcasterUserId, userIdToModerate, validTimeout);
            
            var invalidTimeout = new TimeoutDuration(20000); // Too long - throws exception
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Invalid duration: {ex.Message}");
        }
        
        // Example 6: Timeout a user using plain int (minutes)
        await api.Moderation.TimeoutUserAsync(broadcasterUserId, userIdToModerate, 5);
        
        // Example 7: Permanently ban a user
        var banResult = await api.Moderation.BanUserAsync(
            broadcasterUserId: broadcasterUserId,
            userIdToBan: userIdToModerate,
            reason: "Repeated violations of chat rules"
        );
        
        if (banResult.IsSuccess)
        {
            Console.WriteLine($"✓ User permanently banned");
        }
        
        // Example 8: Unban a user (removes permanent ban or timeout)
        var unbanResult = await api.Moderation.UnbanUserAsync(
            broadcasterUserId: broadcasterUserId,
            userIdToUnban: userIdToModerate
        );
        
        if (unbanResult.IsSuccess)
        {
            Console.WriteLine($"✓ User unbanned successfully");
        }
        
        Console.WriteLine("✓ Moderation examples completed");
    }

    /// <summary>
    /// Example 13: Error Handling
    /// </summary>
    public static async Task Example13_ErrorHandling(IKickApi api)
    {
        var result = await api.Channels.GetChannelAsync("nonexistent-channel-12345");
        
        if (result.IsSuccess)
        {
            Console.WriteLine($"Channel found: {result.Value.Slug}");
        }
        else
        {
            Console.WriteLine("Failed to fetch channel:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  - {error.Message}");
            }
        }
    }

    /// <summary>
    /// Complete workflow example combining multiple features
    /// </summary>
    public static async Task Example14_CompleteWorkflow()
    {
        // 1. Setup - Requires `Microsoft.Extensions.Logging.Console` package
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var settings = new ApiSettings
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret,
            AccessToken = "YOUR_ACCESS_TOKEN",
            RefreshToken = "YOUR_REFRESH_TOKEN"
        };
        
        // Subscribe to token change events to persist tokens
        settings.AccessTokenChanged += (_, args) =>
        {
            Console.WriteLine($"Access token updated: {args.NewToken}");
            // Save to database/file for persistence
        };
        
        settings.RefreshTokenChanged += (_, args) =>
        {
            Console.WriteLine($"Refresh token updated: {args.NewToken}");
            // Save to database/file for persistence
        };
        
        IKickApi api = KickApi.Create(settings, loggerFactory);
        
        // 2. Get authenticated user info
        var meResult = await api.Users.GetMeAsync();
        if (meResult.IsSuccess)
        {
            Console.WriteLine($"✓ Authenticated as: {meResult.Value.Name}\n");
        }
        
        // 3. Get channel info
        var channelResult = await api.Channels.GetChannelAsync("kicklib");
        if (channelResult.IsSuccess)
        {
            Console.WriteLine($"✓ Fetched channel: {channelResult.Value.Slug}");
            Console.WriteLine($"  Is Live: {channelResult.Value.Stream.IsLive}\n");
        }
        
        // 4. Check livestreams
        var livestreamsResult = await api.Livestreams.GetLivestreamsAsync();
        if (livestreamsResult.IsSuccess)
        {
            Console.WriteLine($"✓ Currently live: {livestreamsResult.Value.Count} streams\n");
        }
        
        // 5. Setup webhook subscriptions
        var subscribeResult = await api.EventSubscriptions.SubscribeToEventsAsync(new List<EventType>
        {
            EventType.ChatMessageSent,
            EventType.ChannelFollowed
        });
        
        if (subscribeResult.IsSuccess)
        {
            Console.WriteLine($"✓ Subscribed to webhook events\n");
        }
    }
}
