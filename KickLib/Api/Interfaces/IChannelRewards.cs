using KickLib.Models.v1;
using KickLib.Models.v1.ChannelRewards;
using KickLib.Models.v1.ChannelRewards.Redemptions;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Interact with and manipulate channel rewards.
/// </summary>
public interface IChannelRewards
{
    /// <summary>
    ///     Get channel rewards for a broadcaster's channel.
    ///     Channels may have up to 15 rewards, including both enabled and disabled rewards.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:read or channel:rewards:write
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<ChannelReward>>> GetChannelRewardsAsync(
        string? accessToken = null, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Creates a channel reward in the broadcaster's channel. 
    ///     A maximum of 15 rewards can be created, including both enabled and disabled rewards.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    /// </remarks>
    /// <param name="request">The request object containing the parameters for creating the channel reward.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ChannelReward>> CreateChannelRewardAsync(
        CreateChannelRewardRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a channel reward in the broadcaster's channel. 
    ///     A maximum of 15 rewards can be created, including both enabled and disabled rewards.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    /// </remarks>
    /// <param name="cost">The cost of the reward in channel points (must be 1 or greater).</param>
    /// <param name="title">The title of the reward (50 characters max).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ChannelReward>> CreateChannelRewardAsync(
        string title,
        int cost,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a channel reward in the broadcaster's channel. 
    ///     A maximum of 15 rewards can be created, including both enabled and disabled rewards.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    /// </remarks>
    /// <param name="cost">The cost of the reward in channel points (must be 1 or greater).</param>
    /// <param name="title">The title of the reward (50 characters max).</param>
    /// <param name="isEnabled">Whether the reward is enabled.</param>
    /// <param name="isUserInputRequired">Whether user input is required when redeeming the reward.</param>
    /// <param name="shouldRedemptionsSkipQueue">Whether redemptions of this reward should skip the request queue.</param>
    /// <param name="backgroundColor">The background color of the reward in hexadecimal format.</param>
    /// <param name="description">The description of the reward (200 characters max).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ChannelReward>> CreateChannelRewardAsync(
        string title,
        int cost,
        bool isEnabled,
        bool isUserInputRequired = false,
        bool shouldRedemptionsSkipQueue = false,
        string? backgroundColor = null,
        string? description = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a channel reward in the broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Only the app that created the reward can delete it.
    /// </remarks>
    /// <param name="rewardId">The ID of the reward to be deleted.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<bool>> DeleteChannelRewardAsync(
        string rewardId,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a channel reward in the broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Only the app that created the reward can update it.
    /// </remarks>
    /// <param name="rewardId">The ID of the reward to be updated.</param>
    /// <param name="request">The request object containing the parameters for updating the channel reward.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ChannelReward>> UpdateChannelRewardAsync(
        string rewardId,
        UpdateChannelRewardRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Get channel reward redemptions for a broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<ChannelRewardRedemption>>>> GetChannelRewardRedemptionsAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get channel reward redemptions for a broadcaster's channel with optional filters and pagination.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    /// </remarks>
    /// <param name="rewardId">Filter by Reward ID.</param>
    /// <param name="redemptionStatus">Filter by Redemption Status.</param>
    /// <param name="cursor">Cursor for pagination. Used to get the next set of results.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<ChannelRewardRedemption>>>> GetChannelRewardRedemptionsAsync(
        string? rewardId,
        RedemptionStatus? redemptionStatus,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get channel reward redemptions for a broadcaster's channel by specific redemption IDs.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    /// </remarks>
    /// <param name="redemptionIds">Collection of Redemption IDs to retrieve.</param>
    /// <param name="cursor">Cursor for pagination. Used to get the next set of results.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<ChannelRewardRedemption>>>> GetChannelRewardRedemptionsAsync(
        ICollection<string> redemptionIds,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Accept channel reward redemption for a broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    ///     The response will only include data for redemptions that failed to be accepted.
    /// </remarks>
    /// <param name="redemptionId">Redemption ID to accept.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<FailedRedemptions>> AcceptChannelRewardRedemptionAsync(
        string redemptionId,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Accept channel reward redemptions for a broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    ///     The response will only include data for redemptions that failed to be accepted.
    /// </remarks>
    /// <param name="redemptionIds">A list of Redemption IDs to accept (min: 1, maximum: 25)</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<FailedRedemptions>>> AcceptChannelRewardRedemptionsAsync(
        ICollection<string> redemptionIds,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Reject channel reward redemption for a broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    ///     The response will only include data for redemptions that failed to be rejected.
    /// </remarks>
    /// <param name="redemptionId">Redemption ID to reject.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<FailedRedemptions>> RejectChannelRewardRedemptionAsync(
        string redemptionId,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Reject channel reward redemptions for a broadcaster's channel.
    /// </summary>
    /// <remarks>
    ///     Required scope: channel:rewards:write
    ///     The response will only include data for redemptions that failed to be rejected.
    /// </remarks>
    /// <param name="redemptionIds">A list of Redemption IDs to reject (min: 1, maximum: 25)</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<FailedRedemptions>>> RejectChannelRewardRedemptionsAsync(
        ICollection<string> redemptionIds,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}