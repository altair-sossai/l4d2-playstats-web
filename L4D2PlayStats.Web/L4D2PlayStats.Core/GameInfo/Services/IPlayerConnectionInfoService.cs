using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Results;

namespace L4D2PlayStats.Core.GameInfo.Services;

public interface IPlayerConnectionInfoService
{
    Task<List<PlayerConnectionInfoResult>> RelatedPlayerConnectionInfoAsync(long communityId, CancellationToken cancellationToken = default);
    Task<List<PlayerConnectionInfoResult>> AddOrUpdateAsync(PlayerConnectionInfoCommand command, CancellationToken cancellationToken = default);
    Task DeleteExpiredAsync(DateTimeOffset lastConnectedAtUtc, CancellationToken cancellationToken = default);
}