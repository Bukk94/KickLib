using KickLib.Models.v1.EventSubscriptions;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Manage event subscriptions.
/// </summary>
public interface IEventSubscriptions
{
    /// <summary>
    ///     Get active event subscriptions for given account.
    /// </summary>
    Task<Result<ICollection<EventSubscriptionResponse>>> GetEventSubscriptionsAsync(string? accessToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribe to all available event subscriptions for given account.
    /// </summary>
    Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToAllEventsAsync(string? accessToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribe to v1 event subscriptions for given account.
    /// </summary>
    Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(ICollection<EventType> eventTypes, string? accessToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribe to event subscriptions for given account.
    /// </summary>
    Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(ICollection<EventType> eventTypes, int version, string? accessToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a specific event subscription for given account.
    /// </summary>
    Task<Result<bool>> UnsubscribeEventsAsync(string subscriptionId, string? accessToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete specific event subscriptions for given account.
    /// </summary>
    Task<Result<bool>> UnsubscribeEventsAsync(ICollection<string> subscriptionIds, string? accessToken = null, CancellationToken cancellationToken = default);
}
