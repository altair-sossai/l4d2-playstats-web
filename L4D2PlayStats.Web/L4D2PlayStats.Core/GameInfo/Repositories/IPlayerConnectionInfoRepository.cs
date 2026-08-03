using Azure;
using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.GameInfo.Repositories;

public interface IPlayerConnectionInfoRepository
{
    Task<PlayerConnectionInfoEntity?> FindAsync(string ipAddress, long communityId, CancellationToken cancellationToken = default);
    Task<List<PlayerConnectionInfoEntity>> GetAllByCommunityIdAsync(long communityId, CancellationToken cancellationToken = default);
    Task<List<PlayerConnectionInfoEntity>> GetAllByIpAddressesAsync(IEnumerable<string> ipAddresses, CancellationToken cancellationToken = default);
    Task<List<PlayerConnectionInfoEntity>> GetAllBeforeAsync(DateTimeOffset lastConnectedAtUtc, int count, CancellationToken cancellationToken = default);
    Task AddAsync(PlayerConnectionInfoEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(PlayerConnectionInfoEntity entity, ETag etag, CancellationToken cancellationToken = default);
    Task DeleteAsync(PlayerConnectionInfoEntity entity, CancellationToken cancellationToken = default);
}