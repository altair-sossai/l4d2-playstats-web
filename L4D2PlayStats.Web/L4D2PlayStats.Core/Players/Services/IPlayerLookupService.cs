using L4D2PlayStats.Core.Players.Results;

namespace L4D2PlayStats.Core.Players.Services;

public interface IPlayerLookupService
{
    Task<PlayerLookupResult?> LookupAsync(string? value, CancellationToken cancellationToken = default);
}
