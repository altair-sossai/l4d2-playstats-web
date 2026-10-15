using L4D2PlayStats.Core.ChatBans.Results;

namespace L4D2PlayStats.Web.Models;

public class ChatBansModel(PagedListModel<ChatBanResult> bans, string? query, int totalBans)
{
    public PagedListModel<ChatBanResult> Bans { get; } = bans;
    public string? Query { get; } = query;
    public int TotalBans { get; } = totalBans;
}
