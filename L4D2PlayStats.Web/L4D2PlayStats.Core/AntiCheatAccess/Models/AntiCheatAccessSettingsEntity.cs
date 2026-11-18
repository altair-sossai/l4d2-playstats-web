using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.AntiCheatAccess.Models;

public class AntiCheatAccessSettingsEntity : ITableEntity
{
    public const string DefaultPartitionKey = "AntiCheat";
    public const string DefaultRowKey = "Settings";

    public string? Mode { get; set; }
    public string? UpdatedByCommunityId { get; set; }
    public string? UpdatedByName { get; set; }
    public string? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public string PartitionKey { get; set; } = DefaultPartitionKey;
    public string RowKey { get; set; } = DefaultRowKey;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
