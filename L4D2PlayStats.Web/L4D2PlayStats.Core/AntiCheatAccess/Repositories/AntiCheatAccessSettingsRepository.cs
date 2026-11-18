using Azure.Data.Tables;
using L4D2PlayStats.Core.AntiCheatAccess.Models;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.AntiCheatAccess.Repositories;

public class AntiCheatAccessSettingsRepository(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : BaseTableStorageRepository<AntiCheatAccessSettingsEntity>("AntiCheatSettings", tableContext), IAntiCheatAccessSettingsRepository
{
    private const string CacheKey = "AntiCheatAccessSettings";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

    public async Task<AntiCheatAccessSettingsEntity?> GetAsync(CancellationToken cancellationToken = default)
    {
        if (memoryCache.TryGetValue(CacheKey, out AntiCheatAccessSettingsEntity? cached))
            return cached;

        var entity = await FindAsync(AntiCheatAccessSettingsEntity.DefaultPartitionKey, AntiCheatAccessSettingsEntity.DefaultRowKey, cancellationToken);

        memoryCache.Set(CacheKey, entity, CacheDuration);

        return entity;
    }

    public async Task SaveAsync(AntiCheatAccessSettingsEntity entity, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);

        await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace, cancellationToken);

        memoryCache.Remove(CacheKey);
    }
}
