using L4D2PlayStats.Core.AntiCheat.Responses;

namespace L4D2PlayStats.Core.AntiCheat.Services.Cache;

public interface IAntiCheatServiceCached
{
    Task<ClientVersionResponse?> GetLatestVersionAsync(CancellationToken cancellationToken);
}