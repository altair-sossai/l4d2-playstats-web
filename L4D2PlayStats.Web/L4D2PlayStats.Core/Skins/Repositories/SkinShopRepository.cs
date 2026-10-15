using Azure.Data.Tables;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using L4D2PlayStats.Core.Skins.Models;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Skins.Repositories;

public class SkinShopRepository(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : BaseTableStorageRepository("SkinShop", tableContext), ISkinShopRepository
{
    private const string AllPurchasesCacheKey = "SkinShop_AllPurchases";
    private const string AllEquipmentCacheKey = "SkinShop_AllEquipment";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public Task<SkinWalletEntity?> FindWalletAsync(string communityId, CancellationToken cancellationToken = default)
    {
        return FindAsync<SkinWalletEntity>(communityId, SkinWalletEntity.DefaultRowKey, cancellationToken);
    }

    public Task<SkinPurchaseEntity?> FindPurchaseAsync(string communityId, string skinId, CancellationToken cancellationToken = default)
    {
        return FindAsync<SkinPurchaseEntity>(communityId, SkinPurchaseEntity.RowKeyOf(skinId), cancellationToken);
    }

    public Task<SkinEquipmentEntity?> FindEquipmentAsync(string communityId, string weapon, CancellationToken cancellationToken = default)
    {
        return FindAsync<SkinEquipmentEntity>(communityId, SkinEquipmentEntity.RowKeyOf(weapon), cancellationToken);
    }

    public Task<List<SkinPurchaseEntity>> GetPurchasesAsync(string communityId, CancellationToken cancellationToken = default)
    {
        return QueryAsync<SkinPurchaseEntity>(communityId, SkinPurchaseEntity.RowKeyPrefix, cancellationToken);
    }

    public Task<List<SkinEquipmentEntity>> GetEquipmentAsync(string communityId, CancellationToken cancellationToken = default)
    {
        return QueryAsync<SkinEquipmentEntity>(communityId, SkinEquipmentEntity.RowKeyPrefix, cancellationToken);
    }

    public Task<List<SkinPurchaseEntity>> GetAllPurchasesAsync(CancellationToken cancellationToken = default)
    {
        return memoryCache.GetOrCreateAsync(AllPurchasesCacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;

            return QueryAsync<SkinPurchaseEntity>(null, SkinPurchaseEntity.RowKeyPrefix, cancellationToken);
        })!;
    }

    public Task<List<SkinEquipmentEntity>> GetAllEquipmentAsync(CancellationToken cancellationToken = default)
    {
        return memoryCache.GetOrCreateAsync(AllEquipmentCacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;

            return QueryAsync<SkinEquipmentEntity>(null, SkinEquipmentEntity.RowKeyPrefix, cancellationToken);
        })!;
    }

    public async Task SubmitAsync(IReadOnlyCollection<TableTransactionAction> actions, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);

        try
        {
            await tableClient.SubmitTransactionAsync(actions, cancellationToken);
        }
        finally
        {
            memoryCache.Remove(AllPurchasesCacheKey);
            memoryCache.Remove(AllEquipmentCacheKey);
        }
    }

    private async Task<TEntity?> FindAsync<TEntity>(string partitionKey, string rowKey, CancellationToken cancellationToken)
        where TEntity : class, ITableEntity, new()
    {
        var tableClient = await GetTableClientAsync(cancellationToken);
        var response = await tableClient.GetEntityIfExistsAsync<TEntity>(partitionKey, rowKey, cancellationToken: cancellationToken);

        return response.HasValue ? response.Value : null;
    }

    private async Task<List<TEntity>> QueryAsync<TEntity>(string? partitionKey, string rowKeyPrefix, CancellationToken cancellationToken)
        where TEntity : class, ITableEntity, new()
    {
        var upperBound = rowKeyPrefix[..^1] + (char)(rowKeyPrefix[^1] + 1);

        var filter = partitionKey == null
            ? TableClient.CreateQueryFilter($"RowKey ge {rowKeyPrefix} and RowKey lt {upperBound}")
            : TableClient.CreateQueryFilter($"PartitionKey eq {partitionKey} and RowKey ge {rowKeyPrefix} and RowKey lt {upperBound}");

        var tableClient = await GetTableClientAsync(cancellationToken);
        var entities = new List<TEntity>();

        await foreach (var entity in tableClient.QueryAsync<TEntity>(filter, cancellationToken: cancellationToken))
            entities.Add(entity);

        return entities;
    }
}