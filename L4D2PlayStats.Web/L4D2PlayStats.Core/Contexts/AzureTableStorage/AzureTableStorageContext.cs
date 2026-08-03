using System.Collections.Concurrent;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Core.Contexts.AzureTableStorage;

public class AzureTableStorageContext(IConfiguration configuration) : IAzureTableStorageContext
{
    private static readonly ConcurrentDictionary<string, Task<TableClient>> TableClients = [];
    private string ConnectionString => configuration.GetValue<string>("AzureWebJobsStorage") ?? throw new InvalidOperationException("AzureWebJobsStorage is not configured.");
    private TableServiceClient TableServiceClient => field ??= new TableServiceClient(ConnectionString);

    public async Task<TableClient> GetTableClientAsync(string tableName, CancellationToken cancellationToken = default)
    {
        var creationTask = TableClients.GetOrAdd(tableName, CreateTableClientAsync);

        try
        {
            return await creationTask.WaitAsync(cancellationToken);
        }
        catch
        {
            if (TableClients.TryGetValue(tableName, out var currentTask) && ReferenceEquals(currentTask, creationTask))
                TableClients.TryRemove(tableName, out _);

            throw;
        }
    }

    private async Task<TableClient> CreateTableClientAsync(string tableName)
    {
        var tableClient = TableServiceClient.GetTableClient(tableName);

        await tableClient.CreateIfNotExistsAsync();

        return tableClient;
    }
}