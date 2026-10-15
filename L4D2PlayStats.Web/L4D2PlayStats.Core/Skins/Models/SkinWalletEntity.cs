using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.Skins.Models;

public class SkinWalletEntity : ITableEntity
{
    public const string DefaultRowKey = "Wallet";

    public int Spent { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = DefaultRowKey;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}