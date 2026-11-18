using L4D2PlayStats.Core.AntiCheatAccess.Models;

namespace L4D2PlayStats.Core.AntiCheatAccess.Repositories;

public interface IAntiCheatAccessSettingsRepository
{
    Task<AntiCheatAccessSettingsEntity?> GetAsync(CancellationToken cancellationToken = default);
    Task SaveAsync(AntiCheatAccessSettingsEntity entity, CancellationToken cancellationToken = default);
}
