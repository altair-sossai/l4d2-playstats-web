using Azure;
using Azure.Data.Tables;

namespace L4D2PlayStats.Core.ChatBans.Models;

public class ChatBanEntity : ITableEntity
{
    public const string DefaultPartitionKey = "ChatBan";

    public string LastName { get; set; } = string.Empty;
    public string? SteamId { get; set; }
    public string? Steam3 { get; set; }
    public string? ProfileUrl { get; set; }
    public string? Reason { get; set; }
    public string? CreatedByCommunityId { get; set; }
    public string? CreatedByName { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? UpdatedAtUtc { get; set; }
    public string PartitionKey { get; set; } = string.Empty;
    public string RowKey { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}