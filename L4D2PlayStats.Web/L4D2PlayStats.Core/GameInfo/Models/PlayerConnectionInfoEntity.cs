using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.GameInfo.Models;

public class PlayerConnectionInfoEntity : ITableEntity
{
    public string LastName { get; set; } = string.Empty;
    public string? SteamId { get; set; }
    public string? Steam3 { get; set; }
    public string? ProfileUrl { get; set; }
    public DateTimeOffset FirstConnectedAtUtc { get; set; }
    public DateTimeOffset LastConnectedAtUtc { get; set; }
    public long ConnectionCount { get; set; }
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}