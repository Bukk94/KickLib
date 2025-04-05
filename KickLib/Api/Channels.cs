using KickLib.Models.v1.Channels;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc />
public class Channels : ApiBase
{
    private const string ApiUrlPart = "channels";

    /// <inheritdoc />
    public Channels(ApiSettings settings, ILogger logger) 
        : base(settings, logger)
    {
    }
    
    /// <summary>
    ///     Retrieve channel information based on provided streamer IDs.
    ///     If no streamer IDs are specified, the information for the currently authorised streamer will be returned by default.
    /// </summary>
    public Task<Result<ICollection<ChannelResponse>>> GetChannelsAsync(
        ICollection<int> broadcasterUserIds, 
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        List<KeyValuePair<string, string>>? query = null;
        if (broadcasterUserIds?.Any() == true)
        {
            query = new List<KeyValuePair<string, string>>();
            foreach (var id in broadcasterUserIds.Distinct())
            {
                query.Add(new("broadcaster_user_id", id.ToString()));
            }
        }
        
        // v1/channels
        return GetAsync<ICollection<ChannelResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Retrieve channel information for the currently authorised user.
    /// </summary>
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
    
    public async Task<Result<bool>> UpdateChannelAsync(
        UpdateChannelRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        // v1/channels
        var result = await PatchAsync(ApiUrlPart, ApiVersion.v1, request, accessToken, cancellationToken).ConfigureAwait(false);

        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.ChannelWrite}");
        }

        return result;
    }
}