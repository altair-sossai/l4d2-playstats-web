using Azure.Data.Tables;
using L4D2PlayStats.Core.AntiCheatAccess.Models;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.AntiCheatAccess.Repositories;

public abstract class AntiCheatAccessPlayerRepository(string tableName, IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : BaseTableStorageRepository<AntiCheatAccessPlayerEntity>(tableName, tableContext), IAntiCheatAccessPlayerRepository
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

    private readonly string _allCacheKey = $"AntiCheatAccess_{tableName}_All";

    public Task<List<AntiCheatAccessPlayerEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return memoryCache.GetOrCreateAsync(_allCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;

            var filter = TableClient.CreateQueryFilter($"PartitionKey eq {AntiCheatAccessPlayerEntity.DefaultPartitionKey}");

            return await GetAllAsync(filter, cancellationToken: cancellationToken);
        })!;
    }

    public new async Task AddAsync(AntiCheatAccessPlayerEntity entity, CancellationToken cancellationToken = default)
    {
        await base.AddAsync(entity, cancellationToken);

        memoryCache.Remove(_allCacheKey);
    }

    public new async Task DeleteAsync(AntiCheatAccessPlayerEntity entity, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);

        await tableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey, cancellationToken: cancellationToken);

        memoryCache.Remove(_allCacheKey);
    }
}

public class AntiCheatWhitelistRepository(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : AntiCheatAccessPlayerRepository("AntiCheatWhitelist", tableContext, memoryCache), IAntiCheatWhitelistRepository;

public class AntiCheatRequiredRepository(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : AntiCheatAccessPlayerRepository("AntiCheatRequired", tableContext, memoryCache), IAntiCheatRequiredRepository;
