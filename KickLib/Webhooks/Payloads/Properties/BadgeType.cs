namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents the type of user badge.
/// </summary>
public enum BadgeType
{
    /// <summary>
    ///     Unknown type is not a valid badge type for the Kick. If you see this type, it means that the badge type is not recognized by KickLib.
    /// </summary>
    Unknown,
    
    /// <summary>
    ///     Subscriber badge type, representing a user who has subscribed to the channel.
    /// </summary>
    Subscriber,

    /// <summary>
    ///     Founder badge type, representing one of the first subscribers of the channel.
    /// </summary>
    Founder,

    /// <summary>
    ///     Broadcaster badge type, representing the owner of the channel.
    /// </summary>
    Broadcaster,

    /// <summary>
    ///     OG badge type, representing an original or early supporter of the platform.
    /// </summary>
    Og,

    /// <summary>
    ///     Moderator badge type, representing a user with moderation privileges in the channel.
    /// </summary>
    Moderator,

    /// <summary>
    ///     SubGifter badge type, representing a user who has gifted subscriptions to others.
    /// </summary>
    SubGifter,

    /// <summary>
    ///     Verified badge type, representing a verified user or account.
    /// </summary>
    Verified,

    /// <summary>
    ///     VIP badge type, representing a user with VIP status in the channel.
    /// </summary>
    Vip
}