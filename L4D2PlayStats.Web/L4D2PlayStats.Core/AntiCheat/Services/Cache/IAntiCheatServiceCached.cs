namespace L4D2PlayStats.Core.AntiCheat.Services.Cache;

public interface IAntiCheatServiceCached
{
    Task<string?> GetLatestVersionAsync(CancellationToken cancellationToken);
}