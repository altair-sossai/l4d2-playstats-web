using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.AntiCheatAccess.Models;

public class AntiCheatAccessPlayerEntity : ITableEntity
{
    public const string DefaultPartitionKey = "Player";

    public string LastName { get; set; } = string.Empty;
    public string? SteamId { get; set; }
    public string? Steam3 { get; set; }
    public string? ProfileUrl { get; set; }
    public string? AddedByCommunityId { get; set; }
    public string? AddedByName { get; set; }
    public string? AddedFrom { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public string PartitionKey { get; set; } = DefaultPartitionKey;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
