using Azure;
using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.GameInfo.Repositories;

public interface IPlayerConnectionInfoRepository
{
    Task<PlayerConnectionInfoEntity?> FindAsync(string ipAddress, string communityId, CancellationToken cancellationToken = default);
    Task AddAsync(PlayerConnectionInfoEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(PlayerConnectionInfoEntity entity, ETag etag, CancellationToken cancellationToken = default);
}