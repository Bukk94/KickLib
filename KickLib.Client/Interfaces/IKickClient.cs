using KickLib.Client.Models.Args;

namespace KickLib.Client.Interfaces;

public interface IKickClient
{
    /// <summary>
    ///     Event that fires when client gets connected.
    /// </summary>
    event EventHandler OnConnected;
    
    /// <summary>
    ///     Event that fires when client gets disconnected.
    /// </summary>
    event EventHandler OnDisconnected;
    
    /// <summary>
    ///     Event that fires when client receives new chat message.
    /// </summary>
    event EventHandler<ChatMessageEventArgs> OnMessage;
    
    /// <summary>
    ///     Event that fires when client receives followers update.
    /// </summary>
    event EventHandler<FollowersUpdatedEventArgs> OnFollowersUpdated;
    
    /// <summary>
    ///     Event that fires when client detects stream status change (went online / offline).
    /// </summary>
    event EventHandler<StreamStateChangedArgs> OnStreamStatusChanged;
    
    /// <summary>
    ///     Event that fires when client receives unknown message.
    /// </summary>
    event EventHandler<UnknownEventArgs> OnUnknownEvent;

    /// <summary>
    ///     Listens to events of specific channel.
    /// </summary>
    /// <param name="channelId">Channel ID (also referred as user ID).</param>
    Task ListenToChannelAsync(int channelId);
    
    /// <summary>
    ///     Stops listening to events of specific channel.
    /// </summary>
    /// <param name="channelId">Channel ID (also referred as user ID).</param>
    Task StopListeningToChannelAsync(int channelId);
    
    /// <summary>
    ///     Listens to events of specific chatroom.
    /// </summary>
    /// <param name="chatroomId">Chatroom ID</param>
    Task ListenToChatRoomAsync(int chatroomId);
    
    /// <summary>
    ///     Stops listening to events of specific chatroom.
    /// </summary>
    /// <param name="chatroomId">Chatroom ID</param>
    Task StopListeningToChatRoomAsync(int chatroomId);

    /// <summary>
    ///     Connects to the Kick's server to start listening on events.
    ///     Listeners must be setup before calling this method.
    /// </summary>
    Task ConnectAsync();
    
    /// <summary>
    ///     Disconnects the client.
    /// </summary>
    Task DisconnectAsync();
}