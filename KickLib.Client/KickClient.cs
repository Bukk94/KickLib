using KickLib.Client.Interfaces;
using KickLib.Client.Models.Args;
using KickLib.Client.Models.Events.Channel;
using KickLib.Client.Models.Events.Chatroom;
using KickLib.Client.Models.Events.Livestream;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PusherClient;

namespace KickLib.Client;

public class KickClient : IKickClient
{
    private const string KickWsKey = "32cbd69e4b950bf97679";
    private readonly Pusher _pusher;
    private readonly ILogger _logger;
    
    public KickClient(ILogger logger = null)
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

    public event EventHandler OnConnected;
    public event EventHandler OnDisconnected;
    public event EventHandler<ChatMessageEventArgs> OnMessage;
    public event EventHandler<FollowersUpdatedEventArgs> OnFollowersUpdated;
    public event EventHandler<StreamStateChangedArgs> OnStreamStatusChanged;
    public event EventHandler<UnknownEventArgs> OnUnknownEvent;

    public bool IsConnected => _pusher.State == ConnectionState.Connected;
    
    public async Task ListenToChannelAsync(int channelId)
    {
        var channel = await _pusher.SubscribeAsync($"channel.{channelId}");
        channel.BindAll(ChannelDataReader);
    }
    
    public Task StopListeningToChannelAsync(int channelId)
    {
        return _pusher.UnsubscribeAsync($"channel.{channelId}");
    }
    
    public async Task ListenToChatRoomAsync(int chatroomId)
    {
        var channel = await _pusher.SubscribeAsync($"chatrooms.{chatroomId}.v2");
        channel.BindAll(ChatRoomDataReader);
    }
    
    public Task StopListeningToChatRoomAsync(int chatroomId)
    {
        return _pusher.UnsubscribeAsync($"chatrooms.{chatroomId}.v2");
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
        OnConnected?.Invoke(this, EventArgs.Empty);
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
                var parsed = ParseData<FollowersUpdatedEvent>(e.Data);
                OnFollowersUpdated?.Invoke(this, new FollowersUpdatedEventArgs
                {
                    Data = parsed
                });
                break;
            case "App\\Events\\StreamerIsLive":
                var livestreamData = ParseData<LivestreamStartedEvent>(e.Data);
                OnStreamStatusChanged?.Invoke(this, new StreamStateChangedArgs { IsLive = true, Data = livestreamData });
                break;
            case "App\\Events\\StopStreamBroadcast":
                OnStreamStatusChanged?.Invoke(this, new StreamStateChangedArgs { IsLive = false });
                break;
            default:
                _logger?.LogInformation("Encountered unknown event during channel reading.");
                OnUnknownEvent?.Invoke(this, new UnknownEventArgs
                {
                    EventName = eventName,
                    RawData = e.Data
                });
                break;
        }
    }
    
    private void ChatRoomDataReader(string eventName, PusherEvent e)
    {
        switch (eventName)
        {
            case "App\\Events\\ChatMessageEvent":
                var parsed = ParseData<ChatMessageEvent>(e.Data);
                OnMessage?.Invoke(this, new ChatMessageEventArgs
                {
                    Data = parsed
                });
                break;
            default:
                _logger?.LogInformation("Encountered unknown event during chatroom reading.");
                OnUnknownEvent?.Invoke(this, new UnknownEventArgs
                {
                    EventName = eventName,
                    RawData = e.Data
                });
                break;
        }
    }

    private static TType ParseData<TType>(string data)
    {
        return JsonConvert.DeserializeObject<TType>(data);
    }
}