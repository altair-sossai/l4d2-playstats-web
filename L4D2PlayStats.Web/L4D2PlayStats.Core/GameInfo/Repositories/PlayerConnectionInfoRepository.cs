using System.Globalization;
using Azure.Data.Tables;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.GameInfo.Repositories;

public class PlayerConnectionInfoRepository(IAzureTableStorageContext tableContext) : BaseTableStorageRepository<PlayerConnectionInfoEntity>("PlayerConnectionInfo", tableContext), IPlayerConnectionInfoRepository
{
    private const int MaxFilterComparisons = 15;

    public Task<PlayerConnectionInfoEntity?> FindAsync(string ipAddress, long communityId, CancellationToken cancellationToken = default)
    {
        var rowKey = communityId.ToString(CultureInfo.InvariantCulture);

        return base.FindAsync(ipAddress, rowKey, cancellationToken);
    }

    public Task<List<PlayerConnectionInfoEntity>> GetAllByCommunityIdAsync(long communityId, CancellationToken cancellationToken = default)
    {
        var rowKey = communityId.ToString(CultureInfo.InvariantCulture);
        var filter = TableClient.CreateQueryFilter($"RowKey eq {rowKey}");

        return GetAllAsync(filter, cancellationToken);
    }

    public async Task<List<PlayerConnectionInfoEntity>> GetAllByIpAddressesAsync(IEnumerable<string> ipAddresses, CancellationToken cancellationToken = default)
    {
        var entities = new List<PlayerConnectionInfoEntity>();

        foreach (var ipAddressBatch in ipAddresses.Distinct(StringComparer.Ordinal).Chunk(MaxFilterComparisons))
        {
            var filter = string.Join(" or ", ipAddressBatch.Select(ipAddress => TableClient.CreateQueryFilter($"PartitionKey eq {ipAddress}")));
            var result = await GetAllAsync(filter, cancellationToken);

            entities.AddRange(result);
        }

        return entities;
    }
}