using L4D2PlayStats.Core.AntiCheatAccess.Enums;
using L4D2PlayStats.Core.AntiCheatAccess.Results;
using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.AntiCheatAccess.Services;

public interface IAntiCheatAccessService
{
    Task<AntiCheatAccessResult> GetAsync(bool loadAvatars = true, CancellationToken cancellationToken = default);
    Task<AntiCheatAccessServerResult> GetServerAsync(CancellationToken cancellationToken = default);
    Task SetModeAsync(AntiCheatAccessMode mode, User actor, AntiCheatAccessSource source, CancellationToken cancellationToken = default);
    Task<AntiCheatAccessPlayerResult> AddAsync(AntiCheatAccessList list, string? player, string? name, User actor, AntiCheatAccessSource source, CancellationToken cancellationToken = default);
    Task<bool> RemoveAsync(AntiCheatAccessList list, string? player, AntiCheatAccessSource source, CancellationToken cancellationToken = default);
}
