using L4D2PlayStats.Core.GameInfo.Commands;

namespace L4D2PlayStats.Core.GameInfo.Services;

public interface IPlayerConnectionInfoService
{
    Task AddOrUpdateAsync(PlayerConnectionInfoCommand command, CancellationToken cancellationToken = default);
}