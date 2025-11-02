using KickLib.Client;
using KickLib.Client.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExampleApp.Examples;

/// <summary>
/// Kick WebSocket Client (IKickClient) Usage Examples
/// 
/// This example demonstrates how to use the KickClient to listen to real-time events
/// from Kick.com using WebSocket connections (Pusher protocol).
/// 
/// The client allows you to:
/// - Listen to chat messages in real-time
/// - Detect stream state changes (online/offline)
/// - Monitor follows, subscriptions, and gifts
/// - Track moderation events (bans, timeouts, message deletions)
/// - Receive pinned messages and reward redemptions
/// 
/// No authentication required for reading public events!
/// </summary>
public static class KickClientExample
{
    /// <summary>
    /// Example 1: Listen to Chat Messages
    /// </summary>
    public static async Task Example1_ListenToChatMessages()
    {
        IKickClient client = new KickClient();
        
        // Subscribe to message event
        client.OnMessage += (sender, e) =>
        {
            var message = e.Data;
            Console.WriteLine($"[{message.CreatedAt}] {message.Sender.Username}: {message.Content}");
            
            // Access additional message properties
            Console.WriteLine($"  Message ID: {message.Id}");
            Console.WriteLine($"  Sender ID: {message.Sender.Id}");
            Console.WriteLine($"  Sender Badges: {string.Join(", ", message.Sender.Identity.Badges.Select(b => b.Type))}");
        };
        
        // Get chatroom ID from channel (you need to fetch this first using Unofficial API or other means)
        var chatroomId = 4704335;
        
        await client.ListenToChatRoomAsync(chatroomId);
        await client.ConnectAsync();
        
        Console.WriteLine("Listening to chat messages. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 2: Monitor Stream Status Changes
    /// </summary>
    public static async Task Example2_MonitorStreamStatus()
    {
        IKickClient client = new KickClient();
        
        // Subscribe to stream status change event
        client.OnStreamStatusChanged += (_, e) =>
        {
            if (e.IsLive)
            {
                Console.WriteLine($"🔴 Stream went ONLINE!");
                Console.WriteLine($"  Title: {e.Data.SessionTitle}");
            }
            else
            {
                Console.WriteLine($"⚫ Stream went OFFLINE");
            }
        };
        
        // Listen to channel events (not chatroom)
        var channelId = 4721174; // Example channel ID
        await client.ListenToChannelAsync(channelId);
        await client.ConnectAsync();
        
        Console.WriteLine("Monitoring stream status. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 3: Track Subscriptions and Gifts
    /// </summary>
    public static async Task Example3_TrackSubscriptions()
    {
        IKickClient client = new KickClient();
        
        // Single subscription event
        client.OnSubscription += (_, e) =>
        {
            var sub = e.Data;
            Console.WriteLine($"🎉 New subscriber: {sub.Username}");
            Console.WriteLine($"  Months: {sub.Months}");
        };
        
        // Gifted subscriptions event
        client.OnGiftedSubscription += (_, e) =>
        {
            var gift = e.Data;
            Console.WriteLine($"🎁 {gift.GifterUsername} gifted {gift.Count} subscriptions!");
            
            foreach (var giftedUsername in gift.GiftedTo)
            {
                Console.WriteLine($"  -> {giftedUsername}");
            }
        };
        
        var chatroomId = 4704335;
        await client.ListenToChatRoomAsync(chatroomId);
        await client.ConnectAsync();
        
        Console.WriteLine("Tracking subscriptions. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 4: Monitor Moderation Events
    /// </summary>
    public static async Task Example4_MonitorModeration()
    {
        IKickClient client = new KickClient();
        
        // Message deleted event
        client.OnMessageDeleted += (_, e) =>
        {
            var deletion = e.Data;
            Console.WriteLine($"🗑️ Message deleted: {deletion.Message.Id}");
        };
        
        // User banned event
        client.OnUserBanned += (_, e) =>
        {
            var ban = e.Data;
            Console.WriteLine($"🔨 User banned: {ban.User.Username}");
            Console.WriteLine($"  Banned by: {ban.BannedBy.Username}");
            Console.WriteLine($"  Permanent: {ban.Permanent}");
        };
        
        // User unbanned event
        client.OnUserUnbanned += (_, e) =>
        {
            var unban = e.Data;
            Console.WriteLine($"✅ User unbanned: {unban.User.Username}");
            Console.WriteLine($"  Unbanned by: {unban.BannedBy.Username}");
        };
        
        var chatroomId = 4704335;
        await client.ListenToChatRoomAsync(chatroomId);
        await client.ConnectAsync();
        
        Console.WriteLine("Monitoring moderation events. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 5: Track Followers
    /// </summary>
    public static async Task Example5_TrackFollowers()
    {
        IKickClient client = new KickClient();
        
        client.OnFollowersUpdated += (_, e) =>
        {
            var follower = e.Data;
            Console.WriteLine($"👤 New follower: {follower.Username}");
            Console.WriteLine($"  Followed at: {follower.CreatedAt}");
            Console.WriteLine($"  Followers count: {follower.FollowersCount}");
        };
        
        // Must listen to channel (not chatroom) for follower events
        var channelId = 4721174;
        await client.ListenToChannelAsync(channelId);
        await client.ConnectAsync();
        
        Console.WriteLine("Tracking new followers. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 6: Monitor Pinned Messages
    /// </summary>
    public static async Task Example6_MonitorPinnedMessages()
    {
        IKickClient client = new KickClient();
        
        // Pinned message created
        client.OnPinnedMessageCreated += (_, e) =>
        {
            var pin = e.Data;
            Console.WriteLine($"📌 Message pinned!");
            Console.WriteLine($"  Content: {pin.Message.Content}");
            Console.WriteLine($"  Author: {pin.Message.Sender.Username}");
            Console.WriteLine($"  Duration: {pin.Duration} seconds");
        };
        
        // Pinned message deleted
        client.OnPinnedMessageDeleted += (_, _) =>
        {
            Console.WriteLine($"📌 Pinned message removed");
        };
        
        var chatroomId = 4704335;
        await client.ListenToChatRoomAsync(chatroomId);
        await client.ConnectAsync();
        
        Console.WriteLine("Monitoring pinned messages. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 7: Track Channel Point Rewards
    /// </summary>
    public static async Task Example7_TrackRewards()
    {
        IKickClient client = new KickClient();
        
        client.OnRewardRedeemed += (_, e) =>
        {
            var reward = e.Data;
            Console.WriteLine($"🏆 Reward redeemed: {reward.RewardTitle}");
            Console.WriteLine($"  Redeemed by: {reward.Username}");
            Console.WriteLine($"  User input: {reward.UserInput ?? "None"}");
        };
        
        var chatroomId = 4704335;
        await client.ListenToChatRoomAsync(chatroomId);
        await client.ConnectAsync();
        
        Console.WriteLine("Tracking reward redemptions. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 8: Monitor Stream Hosts
    /// </summary>
    public static async Task Example8_MonitorStreamHosts()
    {
        IKickClient client = new KickClient();
        
        client.OnStreamHost += (_, e) =>
        {
            var host = e.Data;
            Console.WriteLine($"🎥 Stream host event!");
            Console.WriteLine($"  Host: {host.HostUsername}");
            Console.WriteLine($"  Message: {host.OptionalMessage ?? "None"}");
            Console.WriteLine($"  Number of viewers: {host.NumberViewers}");
        };
        
        var chatroomId = 4704335;
        await client.ListenToChatRoomAsync(chatroomId);
        await client.ConnectAsync();
        
        Console.WriteLine("Monitoring stream hosts. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 9: Track Gifts Leaderboard
    /// </summary>
    public static async Task Example9_TrackGiftsLeaderboard()
    {
        IKickClient client = new KickClient();
        
        client.OnGiftsLeaderboardUpdated += (_, e) =>
        {
            var leaderboard = e.Data;
            Console.WriteLine($"🎁 Gifts leaderboard updated!");
            Console.WriteLine($"  Gifter: {leaderboard.GifterUsername} gifted {leaderboard.GiftedQuantity}");
            
            if (leaderboard.Leaderboard.Any())
            {
                Console.WriteLine("  Top gifters:");
                foreach (var gifter in leaderboard.Leaderboard.Take(5))
                {
                    Console.WriteLine($"    {gifter.Username}: {gifter.Quantity} gifts");
                }
            }
        };
        
        var channelId = 4721174;
        await client.ListenToChannelAsync(channelId);
        await client.ConnectAsync();
        
        Console.WriteLine("Tracking gifts leaderboard. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 10: Handle Connection Events
    /// </summary>
    public static async Task Example10_HandleConnectionEvents()
    {
        IKickClient client = new KickClient();
        
        // Connected event
        client.OnConnected += (_, e) =>
        {
            Console.WriteLine($"✅ Connected to Kick!");
            Console.WriteLine($"  Socket ID: {e.SocketId}");
        };
        
        // Disconnected event
        client.OnDisconnected += (_, _) =>
        {
            Console.WriteLine($"❌ Disconnected from Kick");
        };
        
        await client.ConnectAsync();
        
        Console.WriteLine("Press any key to disconnect...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 11: Handle Unknown Events
    /// </summary>
    public static async Task Example11_HandleUnknownEvents()
    {
        IKickClient client = new KickClient();
        
        // Capture events that aren't yet mapped to specific event types
        client.OnUnknownEvent += (_, e) =>
        {
            Console.WriteLine($"❓ Unknown event received:");
            Console.WriteLine($"  Event: {e.EventName}");
            Console.WriteLine($"  Source: {e.Source}");
            Console.WriteLine($"  Raw Data: {e.RawData}");
        };
        
        var chatroomId = 4704335;
        await client.ListenToChatRoomAsync(chatroomId);
        await client.ConnectAsync();
        
        Console.WriteLine("Listening for unknown events. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 12: Listen to Multiple Channels/Chatrooms
    /// </summary>
    public static async Task Example12_MultipleChannels()
    {
        IKickClient client = new KickClient();
        
        client.OnMessage += (_, e) =>
        {
            Console.WriteLine($"[Chat] {e.Data.Sender.Username}: {e.Data.Content}");
        };
        
        client.OnStreamStatusChanged += (_, e) =>
        {
            var status = e.IsLive ? "ONLINE" : "OFFLINE";
            Console.WriteLine($"[Stream] Status changed to: {status}");
        };
        
        // Listen to multiple channels and chatrooms
        await client.ListenToChannelAsync(4721174);
        await client.ListenToChannelAsync(4599);
        await client.ListenToChatRoomAsync(4704335);
        
        await client.ConnectAsync();
        
        Console.WriteLine("Listening to multiple channels. Press any key to stop...");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Complete workflow example: Full-featured chatbot listener
    /// </summary>
    public static async Task Example13_CompleteWorkflow()
    {
        // Setup with logging
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        var logger = loggerFactory.CreateLogger<IKickClient>();
        
        IKickClient client = new KickClient(logger);
        
        // Subscribe to all relevant events
        client.OnConnected += (_, e) =>
        {
            Console.WriteLine($"✅ Connected! Socket ID: {e.SocketId}\n");
        };
        
        client.OnMessage += (_, e) =>
        {
            var msg = e.Data;
            Console.WriteLine($"💬 {msg.Sender.Username}: {msg.Content}");
        };
        
        client.OnSubscription += (_, e) =>
        {
            Console.WriteLine($"🎉 New sub: {e.Data.Username} ({e.Data.Months} months)");
        };
        
        client.OnGiftedSubscription += (_, e) =>
        {
            Console.WriteLine($"🎁 {e.Data.GifterUsername} gifted {e.Data.Count} subs!");
        };
        
        client.OnFollowersUpdated += (_, e) =>
        {
            Console.WriteLine($"👤 New follower: {e.Data.Username}");
        };
        
        client.OnStreamStatusChanged += (_, e) =>
        {
            var status = e.IsLive ? "🔴 LIVE" : "⚫ OFFLINE";
            Console.WriteLine($"{status}");
        };
        
        client.OnUserBanned += (_, e) =>
        {
            Console.WriteLine($"🔨 {e.Data.User.Username} was banned");
        };
        
        client.OnMessageDeleted += (_, e) =>
        {
            Console.WriteLine($"🗑️ Message deleted: {e.Data.Message.Id}");
        };
        
        // Listen to both channel and chatroom
        var channelId = 4721174;
        var chatroomId = 4704335;
        
        await client.ListenToChannelAsync(channelId);
        await client.ListenToChatRoomAsync(chatroomId);
        await client.ConnectAsync();
        
        Console.WriteLine("Bot is now listening to all events!");
        Console.WriteLine("Press any key to stop...\n");
        Console.ReadKey();
        
        await client.DisconnectAsync();
    }

    /// <summary>
    /// Example 14: Graceful Error Handling
    /// </summary>
    public static async Task Example14_ErrorHandling()
    {
        IKickClient client = new KickClient();
        
        try
        {
            await client.ConnectAsync();
            
            // Try to listen to an invalid chatroom
            await client.ListenToChatRoomAsync(-1);
            
            Console.WriteLine("Listening... Press any key to stop.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error occurred: {ex.Message}");
        }
        finally
        {
            if (client.IsConnected)
            {
                await client.DisconnectAsync();
            }
        }
    }
}
