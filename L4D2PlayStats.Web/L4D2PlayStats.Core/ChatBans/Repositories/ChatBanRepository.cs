using Azure;
using Azure.Data.Tables;
using L4D2PlayStats.Core.ChatBans.Models;
using L4D2PlayStats.Core.Contexts.AzureTableStorage;
using L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.ChatBans.Repositories;

public class ChatBanRepository(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : BaseTableStorageRepository<ChatBanEntity>("ChatBans", tableContext), IChatBanRepository
{
    private const string AllCacheKey = "ChatBans_All";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(12);

    public Task<ChatBanEntity?> FindAsync(string communityId, CancellationToken cancellationToken = default)
    {
        var cacheKey = EntityCacheKey(communityId);

        return memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;

            return await base.FindAsync(ChatBanEntity.DefaultPartitionKey, communityId, cancellationToken);
        });
    }

    public Task<List<ChatBanEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return memoryCache.GetOrCreateAsync(AllCacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;

            var filter = TableClient.CreateQueryFilter($"PartitionKey eq {ChatBanEntity.DefaultPartitionKey}");

            return await GetAllAsync(filter, cancellationToken: cancellationToken);
        })!;
    }

    public new async Task AddAsync(ChatBanEntity entity, CancellationToken cancellationToken = default)
    {
        await base.AddAsync(entity, cancellationToken);

        InvalidateCache(entity.RowKey);
    }

    public new async Task UpdateAsync(ChatBanEntity entity, ETag etag, CancellationToken cancellationToken = default)
    {
        await base.UpdateAsync(entity, etag, cancellationToken);

        InvalidateCache(entity.RowKey);
    }

    public new async Task DeleteAsync(ChatBanEntity entity, CancellationToken cancellationToken = default)
    {
        await base.DeleteAsync(entity, cancellationToken);

        InvalidateCache(entity.RowKey);
    }

    private void InvalidateCache(string communityId)
    {
        memoryCache.Remove(AllCacheKey);
        memoryCache.Remove(EntityCacheKey(communityId));
    }

    private static string EntityCacheKey(string communityId)
    {
        return $"ChatBan_{communityId}";
    }
}