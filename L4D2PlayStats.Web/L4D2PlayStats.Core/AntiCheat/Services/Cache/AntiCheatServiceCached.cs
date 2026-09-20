using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace L4D2PlayStats.Core.AntiCheat.Services.Cache;

public class AntiCheatServiceCached(IAntiCheatService antiCheatService, IMemoryCache memoryCache) : IAntiCheatServiceCached
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan FailureCacheDuration = TimeSpan.FromSeconds(30);

    public async Task<string?> GetLatestVersionAsync(CancellationToken cancellationToken)
    {
        return await memoryCache.GetOrCreateAsync("AntiCheatVersion", async entry =>
        {
            try
            {
                var response = await antiCheatService.GetLatestVersionAsync(cancellationToken);

                entry.AbsoluteExpirationRelativeToNow = string.IsNullOrWhiteSpace(response?.Version) ? FailureCacheDuration : CacheDuration;

                return response?.Version;
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to get the latest Anti-Cheat version");

                entry.AbsoluteExpirationRelativeToNow = FailureCacheDuration;

                return null;
            }
        });
    }
}