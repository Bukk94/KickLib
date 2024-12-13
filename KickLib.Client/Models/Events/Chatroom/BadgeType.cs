namespace KickLib.Client.Models.Events.Chatroom;

public enum BadgeType
{
    /// <summary>
    ///     Unknown type is not a valid badge type for the kick. If you see this type, it means that the badge type is not recognized by KickLib.
    /// </summary>
    Unknown,
    
    Subscriber,
    Founder,
    Broadcaster,
    Og,
    Moderator,
    SubGifter,
    Verified,
    Vip
}