using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Contexts.AzureTableStorage;

public interface IAzureTableStorageContext
{
    Task<TableClient> GetTableClientAsync(string tableName, CancellationToken cancellationToken = default);
}