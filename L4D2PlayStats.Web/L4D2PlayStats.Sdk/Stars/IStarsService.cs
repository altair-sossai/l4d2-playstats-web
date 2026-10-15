using L4D2PlayStats.Sdk.Stars.Results;
using Refit;

namespace L4D2PlayStats.Sdk.Stars;

public interface IStarsService
{
    [Get("/api/stars/{serverId}/{communityId}")]
    Task<PlayerStarsResult> GetAsync(string serverId, long communityId, CancellationToken cancellationToken);
}