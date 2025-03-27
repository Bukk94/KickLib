using KickLib.Models.v1.EventSubscriptions;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc />
public class EventSubscriptions : ApiBase
{
    private const string ApiUrlPart = "events/subscriptions";

    /// <inheritdoc />
    public EventSubscriptions(ApiSettings settings, ILogger logger) : base(settings, logger)
    {
    }
    
    /// <summary>
    ///     Get active event subscriptions for given account.
    /// </summary>
    public async Task<Result<ICollection<EventSubscriptionResponse>>> GetEventSubscriptionsAsync(string? accessToken = null)
    {
        // GET v1/events/subscriptions
        var result = await GetAsync<ICollection<EventSubscriptionResponse>>(ApiUrlPart, ApiVersion.v1, null, accessToken)
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
    public Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToAllEventsAsync(string? accessToken = null)
    {
        var eventTypes = Enum.GetValues<EventType>()
            .Where(x => x != EventType.Unknown)
            .ToList();
        
        return SubscribeToEventsAsync(eventTypes, 1, accessToken);
    }
    
    /// <summary>
    ///     Subscribe to v1 event subscriptions for given account.
    /// </summary>
    public Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(
        ICollection<EventType> eventTypes,
        string? accessToken = null)
    {
        return SubscribeToEventsAsync(eventTypes, 1, accessToken);
    }
    
    /// <summary>
    ///     Subscribe to event subscriptions for given account.
    /// </summary>
    public async Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(
        ICollection<EventType> eventTypes,
        int version,
        string? accessToken = null)
    {
        if (eventTypes?.Any() != true)
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
                accessToken)
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

    public Task<Result<bool>> UnsubscribeEventsAsync(
        string subscriptionId,
        string? accessToken = null)
    {
        if (string.IsNullOrWhiteSpace(subscriptionId))
        {
            throw new ArgumentNullException(nameof(subscriptionId));
        }
        
        return UnsubscribeEventsAsync([subscriptionId], accessToken);
    }
    
    /// <summary>
    ///     Delete specific event subscriptions for given account.
    /// </summary>
    public async Task<Result<bool>> UnsubscribeEventsAsync(
        ICollection<string> subscriptionIds,
        string? accessToken = null)
    {
        if (subscriptionIds?.Any() != true)
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
                accessToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.EventsSubscribe}");
        }

        return result;
    }
}