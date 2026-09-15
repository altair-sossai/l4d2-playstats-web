using Azure;
using L4D2PlayStats.Core.ChatBans.Models;

namespace L4D2PlayStats.Core.ChatBans.Repositories;

public interface IChatBanRepository
{
    Task<ChatBanEntity?> FindAsync(string communityId, CancellationToken cancellationToken = default);
    Task<List<ChatBanEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ChatBanEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ChatBanEntity entity, ETag etag, CancellationToken cancellationToken = default);
    Task DeleteAsync(ChatBanEntity entity, CancellationToken cancellationToken = default);
}