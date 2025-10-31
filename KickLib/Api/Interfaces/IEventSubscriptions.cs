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
    /// <remarks>
    ///     Required scope: events:subscribe
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<EventSubscriptionResponse>>> GetEventSubscriptionsAsync(
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribe to all available event subscriptions for given account.
    /// </summary>
    /// <remarks>
    ///     Required scope: events:subscribe
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToAllEventsAsync(
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribe to specific v1 event subscription for given account.
    /// </summary>
    /// <remarks>
    ///     Required scope: events:subscribe
    /// </remarks>
    /// <param name="eventType">Event type to subscribe to.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<SubscribeToEventResponse>> SubscribeToEventAsync(
        EventType eventType, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribe to specific event subscription for given account.
    /// </summary>
    /// <remarks>
    ///     Required scope: events:subscribe
    /// </remarks>
    /// <param name="eventType">Event type to subscribe to.</param>
    /// <param name="version">Version of the event.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<SubscribeToEventResponse>> SubscribeToEventAsync(
        EventType eventType, 
        int version, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Subscribe to v1 event subscriptions for given account.
    /// </summary>
    /// <remarks>
    ///     Required scope: events:subscribe
    /// </remarks>
    /// <param name="eventTypes">Event types to subscribe to.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(
        ICollection<EventType> eventTypes, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Subscribe to event subscriptions for given account.
    /// </summary>
    /// <remarks>
    ///     Required scope: events:subscribe
    /// </remarks>
    /// <param name="eventTypes">Event types to subscribe to.</param>
    /// <param name="version">Version of the event.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(
        ICollection<EventType> eventTypes, 
        int version, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete a specific event subscription for given account.
    /// </summary>
    /// <remarks>
    ///     Required scope: events:subscribe
    /// </remarks>
    /// <param name="subscriptionId">Subscription ID to unsubscribe from.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<bool>> UnsubscribeEventsAsync(
        string subscriptionId, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Delete specific event subscriptions for given account.
    /// </summary>
    /// <remarks>
    ///     Required scope: events:subscribe
    /// </remarks>
    /// <param name="subscriptionIds">A collection of subscription IDs to unsubscribe from.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<bool>> UnsubscribeEventsAsync(
        ICollection<string> subscriptionIds, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);
}
