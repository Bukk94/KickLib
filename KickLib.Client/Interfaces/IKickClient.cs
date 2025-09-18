using KickLib.Client.Models.Args;
#if NET8_0_OR_GREATER
using System.Collections.Immutable;
#endif

namespace KickLib.Client.Interfaces;

public interface IKickClient
{
    /// <summary>
    ///     Event that fires when client gets connected.
    /// </summary>
    event EventHandler<ClientConnectedArgs> OnConnected;

    /// <summary>
    ///     Event that fires when client gets disconnected.
    /// </summary>
    event EventHandler OnDisconnected;

    /// <summary>
    ///     Fires when new chat message is received.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<ChatMessageEventArgs> OnMessage;

    /// <summary>
    ///     Fires when message is deleted in the chatroom.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<MessageDeletedEventArgs> OnMessageDeleted;

    /// <summary>
    ///     Fires when channel gets new subscription.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<SubscriptionEventArgs> OnSubscription;

    /// <summary>
    ///     Fires when there is gifted subscription in the chatroom.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<GiftedSubscriptionsEventArgs> OnGiftedSubscription;

    /// <summary>
    ///     Fires when user is banned.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<UserBannedEventArgs> OnUserBanned;

    /// <summary>
    ///     Fires when user is unbanned.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<UserUnbannedEventArgs> OnUserUnbanned;

    /// <summary>
    ///     Fires when client receives followers update. Channel must be broadcasting to receive updates.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Channel <see cref="ListenToChannelAsync"/>.
    /// </remarks>
    event EventHandler<FollowersUpdatedEventArgs> OnFollowersUpdated;

    /// <summary>
    ///     Fires when client detects stream status change (went online / offline).
    /// </summary>
    /// <remarks>
    ///     Must be listening to Channel <see cref="ListenToChannelAsync"/>.
    /// </remarks>
    event EventHandler<StreamStateChangedArgs> OnStreamStatusChanged;

    /// <summary>
    ///     Fires when client detects stream status change (went online / offline).
    /// </summary>
    /// <remarks>
    ///     Must be listening to Channel <see cref="ListenToChannelAsync"/>.
    /// </remarks>
    event EventHandler<GiftsLeaderboardUpdatedArgs> OnGiftsLeaderboardUpdated;

    /// <summary>
    ///     Fires when stream host event occurs.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<StreamHostEventArgs> OnStreamHost;


    /// <summary>
    ///     Fires when message is pinned in the chatroom.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<PinnedMessageCreatedEventArgs> OnPinnedMessageCreated;

    /// <summary>
    ///     Fires when pinned message is deleted.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<PinnedMessageDeletedEventArgs> OnPinnedMessageDeleted;

    /// <summary>
    ///     Fires when a channel point reward is redeemed.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Chatroom <see cref="ListenToChatRoomAsync"/>.
    /// </remarks>
    event EventHandler<RewardRedeemedEventArgs> OnRewardRedeemed;

    /// <summary>
    ///     Fires when kicks are gifted in a channel.
    /// </summary>
    /// <remarks>
    ///     Must be listening to Channel <see cref="ListenToChannelAsync"/>.
    /// </remarks>
    event EventHandler<KicksGiftedEventArgs> OnKicksGifted;

    /// <summary>
    ///     Event that fires when client receives unknown message.
    /// </summary>
    event EventHandler<UnknownEventArgs> OnUnknownEvent;

    /// <summary>
    ///     Gets connected state of the client. Returns <c>true</c>, if client is connected.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    ///     Gets Socket ID of the client. Returns <c>null</c>, if client is not connected.
    /// </summary>
    /// <remarks>
    ///     Socket ID is unique identifier of the client connection and is used for private pusher channels authentication.
    /// </remarks>
    string? SocketId { get; }

#if NET8_0_OR_GREATER
    /// <summary>
    ///     List of channels the client is listening to.
    /// </summary>
    public ImmutableHashSet<int> ListeningToChannels { get; }

    /// <summary>
    ///     List of chatrooms the client is listening to.
    /// </summary>
    public ImmutableHashSet<int> ListeningToChatRooms { get; }
#else
    /// <summary>
    ///     List of channels the client is listening to.
    /// </summary>
    public HashSet<int> ListeningToChannels { get; }

    /// <summary>
    ///     List of chatrooms the client is listening to.
    /// </summary>
    public HashSet<int> ListeningToChatRooms { get; }
#endif
    
    /// <summary>
    ///     Listens to events of specific channel.
    ///     Uses the `channel.${channelId}` Pusher channel.
    ///     Contains events directly related to the channel, like stream state, followers, etc.
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
    ///     Uses the `chatrooms.${chatroomId}.v2` Pusher channel.
    ///     Contains events directly related to the chatroom, like new messages, message deletions, polls, subscriptions.
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