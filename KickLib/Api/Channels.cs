using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Channels;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IChannels" />
public class Channels : ApiBase, IChannels
{
    private const string ApiUrlPart = "channels";

    /// <inheritdoc />
    public Channels(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Channels> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }

    /// <inheritdoc />
    public async Task<Result<ChannelResponse>> GetChannelAsync(
        int broadcasterUserId,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var result = await GetChannelsAsync([broadcasterUserId], accessToken, cancellationToken).ConfigureAwait(false);
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRead}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<ChannelResponse>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public Task<Result<ICollection<ChannelResponse>>> GetChannelsAsync(
        ICollection<int> broadcasterUserIds, 
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        List<KeyValuePair<string, string>>? query = null;
        if (broadcasterUserIds?.Count > 0)
        {
            query = [];
            foreach (var id in broadcasterUserIds.Distinct())
            {
                query.Add(new("broadcaster_user_id", id.ToString()));
            }
        }
        
        // v1/channels
        return GetAsync<ICollection<ChannelResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<ChannelResponse>> GetChannelAsync(
        string slug,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slug))
        {
            slug = string.Empty;
        }
        
        var result = await GetChannelsAsync([slug], accessToken, cancellationToken).ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRead}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<ChannelResponse>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public Task<Result<ICollection<ChannelResponse>>> GetChannelsAsync(
        ICollection<string> slugs, 
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        List<KeyValuePair<string, string>>? query = null;
        if (slugs?.Count > 0)
        {
            query = [];
            foreach (var id in slugs.Distinct())
            {
                query.Add(new("slug", id));
            }
        }
        
        // v1/channels
        return GetAsync<ICollection<ChannelResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<ChannelResponse>> GetMyChannelAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/channels
        var result = await GetAsync<ICollection<ChannelResponse>>(ApiUrlPart, ApiVersion.v1, null, accessToken, cancellationToken).ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelRead}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<ChannelResponse>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public async Task<Result<bool>> UpdateChannelAsync(
        UpdateChannelRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var payload = UpdateChannelApiRequest.FromRequest(request);
        
        // v1/channels
        var result = await PatchAsync(ApiUrlPart, ApiVersion.v1, payload, accessToken, cancellationToken).ConfigureAwait(false);

        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelWrite}");
        }

        return result;
    }
}