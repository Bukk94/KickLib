using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.ChannelRewards;
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
}
