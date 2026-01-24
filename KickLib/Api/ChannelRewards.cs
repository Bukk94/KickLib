using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1;
using KickLib.Models.v1.ChannelRewards;
using KickLib.Models.v1.ChannelRewards.Redemptions;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IChannelRewards" />
public class ChannelRewards : ApiBase, IChannelRewards
{
    private const string ApiUrlPart = "channels/rewards";

    /// <inheritdoc />
    public ChannelRewards(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<ChannelRewards> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }

    /// <inheritdoc />
    public async Task<Result<ICollection<ChannelReward>>> GetChannelRewardsAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/channels/rewards
        var result = await GetAsync<ICollection<ChannelReward>>(ApiUrlPart, ApiVersion.v1, null, accessToken, cancellationToken).ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRewardsRead} or {KickScopes.ChannelRewardsWrite}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<ICollection<ChannelReward>>(result.Errors);
        }

        return result;
    }

    /// <inheritdoc />
    public Task<Result<ChannelReward>> CreateChannelRewardAsync(
        string title,
        int cost,
        bool isEnabled = true,
        bool isUserInputRequired = false,
        bool shouldRedemptionsSkipQueue = false,
        string? backgroundColor = null,
        string? description = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var payload = new CreateChannelRewardRequest(cost, title)
        {
            BackgroundColor = backgroundColor,
            Description = description,
            IsEnabled = isEnabled,
            IsUserInputRequired = isUserInputRequired,
            ShouldRedemptionsSkipRequestQueue = shouldRedemptionsSkipQueue
        };
        
        return CreateChannelRewardAsync(payload, accessToken, cancellationToken);
    }

    /// <inheritdoc />
    public Task<Result<ChannelReward>> CreateChannelRewardAsync(
        string title,
        int cost,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        return CreateChannelRewardAsync(
            title, 
            cost, 
            isEnabled: true, 
            accessToken: accessToken, 
            cancellationToken: cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<ChannelReward>> CreateChannelRewardAsync(
        CreateChannelRewardRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var payload = CreateChannelRewardApiRequest.FromRequest(request);
            
        // v1/channels/rewards
        var result = await PostAsync<ChannelReward, CreateChannelRewardApiRequest>(ApiUrlPart, ApiVersion.v1, payload, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRewardsWrite}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<ChannelReward>(result.Errors);
        }

        return result;
    }
    
    /// <inheritdoc />
    public async Task<Result<bool>> DeleteChannelRewardAsync(
        string rewardId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(rewardId))
        {
            throw new ArgumentNullException(nameof(rewardId));
        }
        
        // v1/channels/rewards/{id}
        var url = $"{ApiUrlPart}/{rewardId}";

        var result = await DeleteAsync(url, ApiVersion.v1, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRewardsWrite}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<bool>(result.Errors);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<Result<ChannelReward>> UpdateChannelRewardAsync(
        string rewardId,
        UpdateChannelRewardRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        // v1/channels/{id}
        var url = $"{ApiUrlPart}/{rewardId}";
        var payload = UpdateChannelRewardApiRequest.FromRequest(request);
        
        var result = await PatchAsync<ChannelReward, UpdateChannelRewardApiRequest>(url, ApiVersion.v1, payload, accessToken, cancellationToken)
            .ConfigureAwait(false);

        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelWrite}");
        }
        
        if (result.HasError(x => x.Message == "Response code: 404"))
        {
            result.WithError($"Reward with ID {rewardId} not found.");
        }

        return result;
    }
    
    /// <inheritdoc />
    public Task<Result<PaginatedResponse<ICollection<ChannelRewardRedemption>>>> GetChannelRewardRedemptionsAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/channels/rewards/redemptions
        return ChannelRewardRedemptionsInternalAsync(accessToken, null, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<PaginatedResponse<ICollection<ChannelRewardRedemption>>>> GetChannelRewardRedemptionsAsync(
        string? rewardId,
        RedemptionStatus? redemptionStatus,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/channels/rewards/redemptions
        
        var query = new List<KeyValuePair<string, string>>();
        if (!string.IsNullOrWhiteSpace(rewardId))
        {
            query.Add(new("reward_id", rewardId));
        }
        
        if (redemptionStatus is not null)
        {
            query.Add(new("redemptionStatus", redemptionStatus.ToString().ToLower()));
        }
        
        if (!string.IsNullOrWhiteSpace(cursor))
        {
            query.Add(new("cursor", cursor));
        }
        
        return await ChannelRewardRedemptionsInternalAsync(accessToken, query, cancellationToken).ConfigureAwait(false);
    }
    
    /// <inheritdoc />
    public async Task<Result<PaginatedResponse<ICollection<ChannelRewardRedemption>>>> GetChannelRewardRedemptionsAsync(
        ICollection<string> redemptionIds,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (redemptionIds == null || redemptionIds.Count == 0)
        {
            throw new ArgumentNullException(nameof(redemptionIds));
        }
        
        // v1/channels/rewards/redemptions
        var query = new List<KeyValuePair<string, string>>();
        foreach (var id in redemptionIds.Distinct())
        {
            query.Add(new("id", id));
        }
        
        if (!string.IsNullOrWhiteSpace(cursor))
        {
            query.Add(new("cursor", cursor));
        }
        
        return await ChannelRewardRedemptionsInternalAsync(accessToken, query, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Result<FailedRedemptions>> AcceptChannelRewardRedemptionAsync(
        string redemptionId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(redemptionId))
        {
            throw new ArgumentNullException(nameof(redemptionId));
        }
        
        // v1/channels/rewards/redemptions/accept
        var result = await AcceptChannelRewardRedemptionsAsync([redemptionId], accessToken, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            return Result.Fail<FailedRedemptions>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public async Task<Result<ICollection<FailedRedemptions>>> AcceptChannelRewardRedemptionsAsync(
        ICollection<string> redemptionIds,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (redemptionIds == null || redemptionIds.Count == 0)
        {
            throw new ArgumentNullException(nameof(redemptionIds));
        }
        
        if (redemptionIds.Count > 25)
        {
            throw new ArgumentException("A maximum of 25 redemption IDs can be processed at once.", nameof(redemptionIds));
        }
        
        // v1/channels/rewards/redemptions/accept
        var url = $"{ApiUrlPart}/redemptions/accept";

        var payload = new RewardRedemptionsStatusChangeApiRequest
        {
            Ids = redemptionIds.ToHashSet(),
        };
        
        var result = await PostAsync<ICollection<FailedRedemptions>, RewardRedemptionsStatusChangeApiRequest>(
                url, 
                ApiVersion.v1, 
                payload, 
                accessToken,
                cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRewardsWrite}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<ICollection<FailedRedemptions>>(result.Errors);
        }
        
        return result;
    }

    /// <inheritdoc />
    public async Task<Result<FailedRedemptions>> RejectChannelRewardRedemptionAsync(
        string redemptionId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(redemptionId))
        {
            throw new ArgumentNullException(nameof(redemptionId));
        }
        
        // v1/channels/rewards/redemptions/reject
        var result = await RejectChannelRewardRedemptionsAsync([redemptionId], accessToken, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            return Result.Fail<FailedRedemptions>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public async Task<Result<ICollection<FailedRedemptions>>> RejectChannelRewardRedemptionsAsync(
        ICollection<string> redemptionIds,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (redemptionIds == null || redemptionIds.Count == 0)
        {
            throw new ArgumentNullException(nameof(redemptionIds));
        }
        
        if (redemptionIds.Count > 25)
        {
            throw new ArgumentException("A maximum of 25 redemption IDs can be processed at once.", nameof(redemptionIds));
        }
        
        // v1/channels/rewards/redemptions/reject
        var url = $"{ApiUrlPart}/redemptions/reject";

        var payload = new RewardRedemptionsStatusChangeApiRequest
        {
            Ids = redemptionIds.ToHashSet(),
        };
        
        var result = await PostAsync<ICollection<FailedRedemptions>, RewardRedemptionsStatusChangeApiRequest>(
                url, 
                ApiVersion.v1, 
                payload, 
                accessToken,
                cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRewardsWrite}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<ICollection<FailedRedemptions>>(result.Errors);
        }
        
        return result;
    }
    
    private async Task<Result<PaginatedResponse<ICollection<ChannelRewardRedemption>>>> ChannelRewardRedemptionsInternalAsync(
        string? accessToken, 
        List<KeyValuePair<string, string>>? query,
        CancellationToken cancellationToken)
    {
        const string url = $"{ApiUrlPart}/redemptions";
        
        var result = await GetAsync<ICollection<ChannelRewardRedemption>>(url, ApiVersion.v1, query, accessToken, cancellationToken).ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRewardsRead} or {KickScopes.ChannelRewardsWrite}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<PaginatedResponse<ICollection<ChannelRewardRedemption>>>(result.Errors);
        }

        var pagination = result.Successes.OfType<ResponseMetadata>().FirstOrDefault()?.GetPagination();

        return Result.Ok(new PaginatedResponse<ICollection<ChannelRewardRedemption>>
        {
            Data = result.Value,
            NextCursor = pagination?.NextCursor
        });
    }
}
