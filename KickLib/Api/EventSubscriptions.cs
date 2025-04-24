using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.EventSubscriptions;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IEventSubscriptions" />
public class EventSubscriptions : ApiBase, IEventSubscriptions
{
    private const string ApiUrlPart = "events/subscriptions";

    /// <inheritdoc />
    public EventSubscriptions(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<EventSubscriptions> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }
    
    /// <summary>
    ///     Get active event subscriptions for given account.
    /// </summary>
    public async Task<Result<ICollection<EventSubscriptionResponse>>> GetEventSubscriptionsAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // GET v1/events/subscriptions
        var result = await GetAsync<ICollection<EventSubscriptionResponse>>(ApiUrlPart, ApiVersion.v1, null, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.EventsSubscribe}");
        }

        return result;
    }

    /// <summary>
    ///     Subscribe to all available event subscriptions for given account.
    /// </summary>
    public Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToAllEventsAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var eventTypes = Enum.GetValues<EventType>()
            .Where(x => x != EventType.Unknown)
            .ToList();
        
        return SubscribeToEventsAsync(eventTypes, 1, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Subscribe to v1 event subscriptions for given account.
    /// </summary>
    public Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(
        ICollection<EventType> eventTypes,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        return SubscribeToEventsAsync(eventTypes, 1, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Subscribe to event subscriptions for given account.
    /// </summary>
    public async Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(
        ICollection<EventType> eventTypes,
        int version,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (eventTypes?.Count == 0)
        {
            throw new ArgumentException("At least one event type must be provided!");
        }
        
        if (version < 1)
        {
            throw new ArgumentException("Version must be at least 1!");
        }
        
        var eventsToSubscribe = eventTypes
            .Select(type => new InputSubscribe(type, version))
            .ToList();

        var payload = new SubscribeToEventRequest(eventsToSubscribe);
        
        // POST v1/events/subscriptions
        var result = await PostAsync<ICollection<SubscribeToEventResponse>, SubscribeToEventRequest>(
                ApiUrlPart, 
                ApiVersion.v1, 
                payload, 
                accessToken,
                cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.EventsSubscribe}");
        }
        
        if (result.Value?.Any(x => x.Error?.Contains("webhooks are not enabled for app") == true) == true)
        {
            result.WithError("Webhooks are not enabled for your app! Visit https://kick.com/settings/developer to enable them.");
        }

        return result;
    }

    /// <summary>
    /// Delete a specific subscription for given account.
    /// </summary>
    public Task<Result<bool>> UnsubscribeEventsAsync(
        string subscriptionId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(subscriptionId))
        {
            throw new ArgumentNullException(nameof(subscriptionId));
        }
        
        return UnsubscribeEventsAsync([subscriptionId], accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Delete specific event subscriptions for given account.
    /// </summary>
    public async Task<Result<bool>> UnsubscribeEventsAsync(
        ICollection<string> subscriptionIds,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (subscriptionIds?.Count == 0)
        {
            throw new ArgumentException("At least one subscription ID must be provided!");
        }
        
        var query = subscriptionIds
            .Distinct()
            .Select(id => new KeyValuePair<string, string>("id", id))
            .ToList();

        // DELETE v1/events/subscriptions
        var result = await DeleteAsync(
                ApiUrlPart, 
                ApiVersion.v1, 
                query, 
                accessToken,
                cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.EventsSubscribe}");
        }

        return result;
    }
}