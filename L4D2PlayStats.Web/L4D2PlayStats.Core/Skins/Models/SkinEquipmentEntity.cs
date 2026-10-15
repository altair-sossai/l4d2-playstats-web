using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Skins.Models;

public class SkinEquipmentEntity : ITableEntity
{
    public const string RowKeyPrefix = "Equipped_";

    public string Weapon { get; set; } = string.Empty;
    public string SkinId { get; set; } = string.Empty;
    public string? Source { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public static string RowKeyOf(string weapon)
    {
        return $"{RowKeyPrefix}{weapon}";
    }
}