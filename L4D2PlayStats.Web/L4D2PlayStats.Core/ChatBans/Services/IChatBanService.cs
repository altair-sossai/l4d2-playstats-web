using L4D2PlayStats.Core.ChatBans.Commands;
using L4D2PlayStats.Core.ChatBans.Results;
using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.ChatBans.Services;

public interface IChatBanService
{
    Task<ChatBanResult?> FindAsync(string? communityId, CancellationToken cancellationToken = default);
    Task<List<ChatBanResult>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<RecentChatSenderResult>> GetRecentSendersAsync(CancellationToken cancellationToken = default);
    Task<ChatBanResult> AddOrUpdateAsync(ChatBanCommand command, User admin, CancellationToken cancellationToken = default);
    Task DeleteAsync(string communityId, CancellationToken cancellationToken = default);
}