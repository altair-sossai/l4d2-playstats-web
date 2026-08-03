using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Contexts.AzureTableStorage.Repositories;

public abstract class BaseTableStorageRepository(string tableName, IAzureTableStorageContext tableContext)
{
    protected Task<TableClient> GetTableClientAsync(CancellationToken cancellationToken = default)
    {
        return tableContext.GetTableClientAsync(tableName, cancellationToken);
    }
}

public abstract class BaseTableStorageRepository<TEntity>(string tableName, IAzureTableStorageContext tableContext) : BaseTableStorageRepository(tableName, tableContext)
    where TEntity : class, ITableEntity, new()
{
    public async Task<TEntity?> FindAsync(string partitionKey, string rowKey, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);
        var response = await tableClient.GetEntityIfExistsAsync<TEntity>(partitionKey, rowKey, cancellationToken: cancellationToken);

        return response.HasValue ? response.Value : null;
    }

    protected async Task<List<TEntity>> GetAllAsync(string filter, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);
        var entities = new List<TEntity>();

        await foreach (var entity in tableClient.QueryAsync<TEntity>(filter, cancellationToken: cancellationToken))
            entities.Add(entity);

        return entities;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);

        await tableClient.AddEntityAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, ETag etag, CancellationToken cancellationToken = default)
    {
        var tableClient = await GetTableClientAsync(cancellationToken);

        await tableClient.UpdateEntityAsync(entity, etag, TableUpdateMode.Replace, cancellationToken);
    }
}