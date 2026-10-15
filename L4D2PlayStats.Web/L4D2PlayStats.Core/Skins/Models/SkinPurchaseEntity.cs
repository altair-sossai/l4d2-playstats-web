using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Skins.Models;

public class SkinPurchaseEntity : ITableEntity
{
    public const string RowKeyPrefix = "Owned_";

    public string SkinId { get; set; } = string.Empty;
    public string Weapon { get; set; } = string.Empty;
    public int Price { get; set; }
    public int StarsBefore { get; set; }
    public bool Gift { get; set; }
    public string? PlayerName { get; set; }
    public string? GiftedByCommunityId { get; set; }
    public DateTimeOffset PurchasedAtUtc { get; set; }
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public static string RowKeyOf(string skinId)
    {
        return $"{RowKeyPrefix}{skinId}";
    }
}