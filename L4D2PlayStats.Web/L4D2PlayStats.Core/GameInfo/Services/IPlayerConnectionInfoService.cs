using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Results;

namespace L4D2PlayStats.Core.GameInfo.Services;

public interface IPlayerConnectionInfoService
{
    Task<List<PlayerConnectionInfoResult>> GetRelatedPlayersAsync(long communityId, CancellationToken cancellationToken = default);
    Task AddOrUpdateAsync(PlayerConnectionInfoCommand command, CancellationToken cancellationToken = default);
}