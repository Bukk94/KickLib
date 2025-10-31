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
    
    /// <inheritdoc />
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

    /// <inheritdoc />
    public Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToAllEventsAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
#if NET8_0_OR_GREATER
        var eventTypes = Enum.GetValues<EventType>()
            .Where(x => x != EventType.Unknown)
            .ToList();
#else
        var eventTypes = Enum.GetValues(typeof(EventType))
            .Cast<EventType>()
            .Where(x => x != EventType.Unknown)
            .ToList();
#endif   
        return SubscribeToEventsAsync(eventTypes, 1, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<SubscribeToEventResponse>> SubscribeToEventAsync(
        EventType eventType,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var result = await SubscribeToEventsAsync([eventType], 1, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.IsFailed)
        {
            return Result.Fail<SubscribeToEventResponse>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public async Task<Result<SubscribeToEventResponse>> SubscribeToEventAsync(
        EventType eventType,
        int version,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var result = await SubscribeToEventsAsync([eventType], version, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.IsFailed)
        {
            return Result.Fail<SubscribeToEventResponse>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(
        ICollection<EventType> eventTypes,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        return SubscribeToEventsAsync(eventTypes, 1, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<ICollection<SubscribeToEventResponse>>> SubscribeToEventsAsync(
        ICollection<EventType> eventTypes,
        int version,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
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

    /// <inheritdoc />
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
    
    /// <inheritdoc />
    public async Task<Result<bool>> UnsubscribeEventsAsync(
        ICollection<string> subscriptionIds,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
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