using KickLib.Models.v1.EventSubscriptions;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

public class EventSubscriptions : ApiBase
{
    private const string ApiUrlPart = "events/subscriptions";

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
        
        var payload = eventTypes
            .Select(type => new InputSubscribe(type, version))
            .ToList();
        
        // POST v1/events/subscriptions
        var result = await PostAsync<ICollection<SubscribeToEventResponse>, ICollection<InputSubscribe>>(
                ApiUrlPart, 
                ApiVersion.v1, 
                payload, 
                accessToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.EventsSubscribe}");
        }

        return result;
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