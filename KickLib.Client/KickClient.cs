#if NET8_0_OR_GREATER
using System.Collections.Immutable;
#endif
using KickLib.Client.Interfaces;
using KickLib.Client.Models.Args;
using KickLib.Client.Models.Events.Channel;
using KickLib.Client.Models.Events.Channel.Gifts;
using KickLib.Client.Models.Events.Chatroom;
using KickLib.Client.Models.Events.Chatroom.Pins;
using KickLib.Client.Models.Events.Livestream;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PusherClient;

namespace KickLib.Client;

public class KickClient : IKickClient
{
    private const string KickWsKey = "32cbd69e4b950bf97679";
    private readonly Pusher _pusher;
    private readonly ILogger? _logger;
    
    private readonly HashSet<int> _listeningToChannels = new();
    private readonly HashSet<int> _listeningToChatRooms = new();
    
    public KickClient(ILogger? logger = null)
    {
        _logger = logger;
        
        var options = new PusherOptions
        {
            Cluster = "us2",
            Encrypted = true
        };

        _pusher = new Pusher(KickWsKey, options);
        _pusher.Connected += PusherOnConnected;
        _pusher.Disconnected += PusherOnDisconnected;
    }

    #region Events
    public event EventHandler<ClientConnectedArgs>? OnConnected;
    public event EventHandler? OnDisconnected;
    public event EventHandler<ChatMessageEventArgs>? OnMessage;
    public event EventHandler<MessageDeletedEventArgs>? OnMessageDeleted;
    public event EventHandler<SubscriptionEventArgs>? OnSubscription;
    public event EventHandler<GiftedSubscriptionsEventArgs>? OnGiftedSubscription;
    public event EventHandler<UserBannedEventArgs>? OnUserBanned;
    public event EventHandler<UserUnbannedEventArgs>? OnUserUnbanned;
    public event EventHandler<FollowersUpdatedEventArgs>? OnFollowersUpdated;
    public event EventHandler<StreamStateChangedArgs>? OnStreamStatusChanged;
    public event EventHandler<StreamHostEventArgs>? OnStreamHost;
    public event EventHandler<GiftsLeaderboardUpdatedArgs>? OnGiftsLeaderboardUpdated;
    public event EventHandler<UnknownEventArgs>? OnUnknownEvent;
    public event EventHandler<PinnedMessageCreatedEventArgs>? OnPinnedMessageCreated;
    public event EventHandler<PinnedMessageDeletedEventArgs>? OnPinnedMessageDeleted;
    public event EventHandler<RewardRedeemedEventArgs>? OnRewardRedeemed;
    public event EventHandler<KicksGiftedEventArgs>? OnKicksGifted;
    #endregion
    
    public bool IsConnected => _pusher.State == ConnectionState.Connected;
    public string? SocketId => _pusher.SocketID;
    
#if NET8_0_OR_GREATER
    public ImmutableHashSet<int> ListeningToChannels => _listeningToChannels.ToImmutableHashSet();
    public ImmutableHashSet<int> ListeningToChatRooms => _listeningToChatRooms.ToImmutableHashSet();
#else
    public HashSet<int> ListeningToChannels => _listeningToChannels;
    public HashSet<int> ListeningToChatRooms => _listeningToChatRooms;
#endif
    
    public async Task ListenToChannelAsync(int channelId)
    {
        var channel = await _pusher.SubscribeAsync($"channel.{channelId}").ConfigureAwait(false);
        channel.BindAll(ChannelDataReader);

        var channelAlternative = await _pusher.SubscribeAsync($"channel_{channelId}").ConfigureAwait(false);
        channelAlternative.BindAll(ChannelDataReader);

        _listeningToChannels.Add(channelId);
    }
    
    public Task StopListeningToChannelAsync(int channelId)
    {
        _listeningToChannels.Remove(channelId);
        return Task.WhenAll(
            _pusher.UnsubscribeAsync($"channel.{channelId}"),
            _pusher.UnsubscribeAsync($"channel_{channelId}")
        );
    }
    
    public async Task ListenToChatRoomAsync(int chatroomId)
    {
        var chatroom = await _pusher.SubscribeAsync($"chatroom_{chatroomId}").ConfigureAwait(false);
        chatroom.BindAll(ChatRoomDataReader);
        
        var chatroomsV1 = await _pusher.SubscribeAsync($"chatrooms.{chatroomId}").ConfigureAwait(false);
        chatroomsV1.BindAll(ChatRoomDataReader);
        
        var chatroomsV2 = await _pusher.SubscribeAsync($"chatrooms.{chatroomId}.v2").ConfigureAwait(false);
        chatroomsV2.BindAll(ChatRoomDataReader);
        
        _listeningToChatRooms.Add(chatroomId);
    }

    public Task StopListeningToChatRoomAsync(int chatroomId)
    {
        _listeningToChatRooms.Remove(chatroomId);
        return Task.WhenAll(
            _pusher.UnsubscribeAsync($"chatroom_{chatroomId}"),
            _pusher.UnsubscribeAsync($"chatrooms.{chatroomId}"),
            _pusher.UnsubscribeAsync($"chatrooms.{chatroomId}.v2")
        );
    }  
    
    public Task ConnectAsync()
    {
        var channels = _pusher.GetAllChannels();
        if (!channels.Any())
        {
            const string message = "Could not connect to Kick's client: Nothing to listen to. " +
                                   $"Call '{nameof(ListenToChannelAsync)}' or '{nameof(ListenToChatRoomAsync)}' " +
                                   $"before calling '{nameof(ConnectAsync)}.'";
            
            throw new ArgumentException(message);
        }
        
        return _pusher.ConnectAsync();
    }

    public Task DisconnectAsync()
    {
        return _pusher.DisconnectAsync();
    }
    
    private void PusherOnConnected(object sender)
    {
        _logger?.LogInformation("Client connected to Kick server");
        OnConnected?.Invoke(this, new ClientConnectedArgs
        {
            SocketId = _pusher.SocketID
        });
    }
    
    private void PusherOnDisconnected(object sender)
    {
        _logger?.LogInformation("Client disconnected from Kick server");
        OnDisconnected?.Invoke(this, EventArgs.Empty);
    }

    private void ChannelDataReader(string eventName, PusherEvent e)
    {
        switch (eventName)
        {
            case "App\\Events\\FollowersUpdated":
                var followersUpdated = ParseData<FollowersUpdatedEvent>(e.Data);
                OnFollowersUpdated?.Invoke(this, new FollowersUpdatedEventArgs
                {
                    Data = followersUpdated
                });
                break;
            
            case "App\\Events\\StreamerIsLive":
                var livestreamData = ParseData<LivestreamWrapper<LivestreamChangedEvent>>(e.Data);
                OnStreamStatusChanged?.Invoke(this, new StreamStateChangedArgs
                {
                    IsLive = true,
                    ChannelId = livestreamData.Livestream.ChannelId,
                    Data = livestreamData.Livestream
                });
                break;
            
            case "App\\Events\\StopStreamBroadcast":
                var livestreamEndedData = ParseData<LivestreamWrapper<LivestreamEndedEvent>>(e.Data);
                OnStreamStatusChanged?.Invoke(this, new StreamStateChangedArgs
                {
                    IsLive = false, 
                    Data = new LivestreamChangedEvent
                    {
                        Id = livestreamEndedData.Livestream.Id,
                        ChannelId = livestreamEndedData.Livestream.Channel.Id,
                        CreatedAt = DateTime.UtcNow
                    }
                });
                break;
            
            // case "App\\Events\\StreamHostedEvent": {"event":"App\\Events\\StreamHostedEvent","data":"{\"message\":{\"id\":\"af4673b8-a06f-4d47-84d2-9fb8c75117e2\",\"numberOfViewers\":8,\"optionalMessage\":null,\"createdAt\":\"2025-08-19T09:37:20.859247Z\"},\"user\":{\"id\":2341810,\"username\":\"ABC\",\"isSuperAdmin\":false,\"verified\":false}}","channel":"chatrooms.12345"}
            // case "App\\Events\\ChannelSubscriptionEvent": {\"user_ids\":[1234500],\"username\":\"ABC\",\"channel_id\":1174682}","channel":"channel.1174682"}
            // case "App\\Events\\LuckyUsersWhoGotGiftSubscriptionsEvent":
            
            case "App\\Events\\GiftsLeaderboardUpdated":
                var giftsLeaderboardUpdate = ParseData<GiftsLeaderboardUpdatedEvent>(e.Data);
                OnGiftsLeaderboardUpdated?.Invoke(this, new GiftsLeaderboardUpdatedArgs
                {
                    Data = giftsLeaderboardUpdate
                });
                break;

            case "KicksGifted":
                var kicksGiftedEvent = ParseData<KicksGiftedEvent>(e.Data);
                OnKicksGifted?.Invoke(this, new KicksGiftedEventArgs
                {
                    Data = kicksGiftedEvent
                });
                break;
            
            default:
                _logger?.LogInformation("Encountered unknown event during channel reading.");
                OnUnknownEvent?.Invoke(this, new UnknownEventArgs
                {
                    EventName = eventName,
                    RawData = e.Data,
                    Source = EventSource.Channel
                });
                break;
        }
    }
    
    private void ChatRoomDataReader(string eventName, PusherEvent e)
    {
        switch (eventName)
        {
            case "App\\Events\\ChatMessageEvent":
                var parsedChatEvent = ParseData<ChatMessageEvent>(e.Data);
                OnMessage?.Invoke(this, new ChatMessageEventArgs
                {
                    Data = parsedChatEvent
                });
                break;
            
            case "App\\Events\\MessageDeletedEvent":
                var messageDeletedEvent = ParseData<MessageDeletedEvent>(e.Data);
                OnMessageDeleted?.Invoke(this, new MessageDeletedEventArgs
                {
                    Data = messageDeletedEvent
                });
                break;

            case "SubscriptionEvent":
            case "App\\Events\\SubscriptionEvent":
                var parsedSubscriptionEvent = ParseData<SubscriptionEvent>(e.Data);
                OnSubscription?.Invoke(this, new SubscriptionEventArgs
                {
                    Data = parsedSubscriptionEvent
                });
                break;

            case "GiftedSubscriptionsEvent":
            case "App\\Events\\GiftedSubscriptionsEvent":
                var parsedGiftedSubscriptionEvent = ParseData<GiftedSubscriptionsEvent>(e.Data);
                OnGiftedSubscription?.Invoke(this, new GiftedSubscriptionsEventArgs
                {
                    Data = parsedGiftedSubscriptionEvent
                });
                break;
            
            case "App\\Events\\UserBannedEvent":
                var userBannedEvent = ParseData<UserBannedEvent>(e.Data);
                OnUserBanned?.Invoke(this, new UserBannedEventArgs
                {
                    Data = userBannedEvent
                });
                break;
            
            case "App\\Events\\UserUnbannedEvent":
                var userUnbannedEvent = ParseData<UserUnbannedEvent>(e.Data);
                OnUserUnbanned?.Invoke(this, new UserUnbannedEventArgs
                {
                    Data = userUnbannedEvent
                });
                break;
            
            case "App\\Events\\StreamHostEvent":
                var streamHostEvent = ParseData<StreamHostEvent>(e.Data);
                OnStreamHost?.Invoke(this, new StreamHostEventArgs
                {
                    Data = streamHostEvent
                });
                break;
            
            case "App\\Events\\PinnedMessageCreatedEvent":
                var pinnedMessage = ParseData<PinnedMessageCreatedEvent>(e.Data);
                OnPinnedMessageCreated?.Invoke(this, new PinnedMessageCreatedEventArgs
                {
                    Data = pinnedMessage
                });
                break;
            
            case "App\\Events\\PinnedMessageDeletedEvent":
                OnPinnedMessageDeleted?.Invoke(this, new PinnedMessageDeletedEventArgs());
                break;

            case "RewardRedeemedEvent":
                var rewardRedeemedEvent = ParseData<RewardRedeemedEvent>(e.Data);
                OnRewardRedeemed?.Invoke(this, new RewardRedeemedEventArgs
                {
                    Data = rewardRedeemedEvent
                });
                break;
            
            default:
                _logger?.LogInformation("Encountered unknown event during chatroom reading.");
                OnUnknownEvent?.Invoke(this, new UnknownEventArgs
                {
                    EventName = eventName,
                    RawData = e.Data,
                    Source = EventSource.Chatroom
                });
                break;
        }
    }

    private static TType ParseData<TType>(string data)
    {
        return JsonConvert.DeserializeObject<TType>(data)!;
    }
}