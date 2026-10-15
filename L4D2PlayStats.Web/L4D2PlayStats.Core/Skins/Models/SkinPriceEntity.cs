using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Skins.Models;

public class SkinPriceEntity : ITableEntity
{
    public const string DefaultPartitionKey = "SkinPrice";

    public int Price { get; set; }
    public string? UpdatedByCommunityId { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}