using Azure.Data.Tables;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using L4D2PlayStats.Core.Skins.Models;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Skins.Repositories;

public class SkinPriceRepository(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : BaseTableStorageRepository<SkinPriceEntity>("SkinPrices", tableContext), ISkinPriceRepository
{
    private const string AllCacheKey = "SkinPrices_All";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

    public Task<List<SkinPriceEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return memoryCache.GetOrCreateAsync(AllCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;

            var filter = TableClient.CreateQueryFilter($"PartitionKey eq {SkinPriceEntity.DefaultPartitionKey}");

            return await GetAllAsync(filter, cancellationToken: cancellationToken);
        })!;
    }

    public async Task UpsertAsync(SkinPriceEntity entity, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);

        await tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace, cancellationToken);

        memoryCache.Remove(AllCacheKey);
    }

    public async Task DeleteAsync(string skinId, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);

        await tableClient.DeleteEntityAsync(SkinPriceEntity.DefaultPartitionKey, skinId, cancellationToken: cancellationToken);

        memoryCache.Remove(AllCacheKey);
    }
}