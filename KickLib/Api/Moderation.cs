using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Moderation;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IModeration" />
public class Moderation : ApiBase, IModeration
{
    private const string ApiUrlPart = "moderation/bans";

    /// <inheritdoc />
    public Moderation(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Moderation> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }

    /// <inheritdoc />
    public async Task<Result> BanUserAsync(
        int broadcasterUserId,
        int userIdToBan,
        string? reason = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(reason) &&
            reason.Length > 100)
        {
            return Result.Fail("Reason must be 100 characters or less.");
        }
        
        var payload = new BanUserPayload
        {
            BroadcasterId = broadcasterUserId,
            UserIdToBan = userIdToBan,
            Reason = reason
        };
        
        // v1/moderation/ban
        var result = await PostAsync<object, BanUserPayload>(ApiUrlPart, ApiVersion.v1, payload, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ModerationBan}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        return Result.Ok().WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public async Task<Result> TimeoutUserAsync(
        int broadcasterUserId,
        int userIdToBan,
        int duration,
        string? reason = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(reason) &&
            reason.Length > 100)
        {
            return Result.Fail("Reason must be 100 characters or less.");
        }
        
        if (duration < 1 || duration > 10080)
        {
            return Result.Fail("Duration must be between 1 and 10080 minutes.");
        }
        
        var payload = new BanUserPayload
        {
            BroadcasterId = broadcasterUserId,
            UserIdToBan = userIdToBan,
            Duration = duration,
            Reason = reason
        };
        
        // v1/moderation/ban
        var result = await PostAsync<object, BanUserPayload>(ApiUrlPart, ApiVersion.v1, payload, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ModerationBan}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        return Result.Ok().WithSuccesses(result.Successes);
    }

    /// <inheritdoc />
    public Task<Result> TimeoutUserAsync(
        int broadcasterUserId,
        int userIdToBan,
        TimeoutDuration duration,
        string? reason = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        return TimeoutUserAsync(broadcasterUserId, userIdToBan, duration.Minutes, reason, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result> UnbanUserAsync(
        int broadcasterUserId,
        int userIdToUnban,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var payload = new BanUserPayload
        {
            BroadcasterId = broadcasterUserId,
            UserIdToBan = userIdToUnban
        };
        
        // v1/moderation/ban
        var result = await DeleteAsync(ApiUrlPart, ApiVersion.v1, payload, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ModerationBan}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        return Result.Ok().WithSuccesses(result.Successes);
    }
}