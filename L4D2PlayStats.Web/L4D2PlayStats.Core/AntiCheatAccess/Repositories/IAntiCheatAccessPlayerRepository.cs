using L4D2PlayStats.Core.AntiCheatAccess.Models;

namespace L4D2PlayStats.Core.AntiCheatAccess.Repositories;

public interface IAntiCheatAccessPlayerRepository
{
    Task<List<AntiCheatAccessPlayerEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(AntiCheatAccessPlayerEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(AntiCheatAccessPlayerEntity entity, CancellationToken cancellationToken = default);
}

public interface IAntiCheatWhitelistRepository : IAntiCheatAccessPlayerRepository;

public interface IAntiCheatRequiredRepository : IAntiCheatAccessPlayerRepository;
