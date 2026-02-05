using System.Collections.Concurrent;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using Serilog;

namespace L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;

public class ServerInfoServiceCached(IServerInfoService serverInfoService) : IServerInfoServiceCached
{
    private static readonly ConcurrentDictionary<string, CacheEntry> Cache = new();
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();

    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

    public Task<GetServerListResponse?> GetServerInfoAsync(string key, string filter, CancellationToken cancellationToken)
    {
        Cache.TryGetValue(filter, out var entry);

        TryRefreshAsync(key, filter, entry, cancellationToken);

        return Task.FromResult(entry?.Value);
    }

    private void TryRefreshAsync(string key, string filter, CacheEntry? entry, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        if (entry != null && now - entry.LastUpdate < RefreshInterval)
            return;

        var semaphoreSlim = Locks.GetOrAdd(filter, _ => new SemaphoreSlim(1, 1));

        _ = Task.Run(async () =>
        {
            if (!await semaphoreSlim.WaitAsync(0, cancellationToken))
                return;

            try
            {
                if (Cache.TryGetValue(filter, out var current) && now - current.LastUpdate < RefreshInterval)
                    return;

                var response = await serverInfoService.GetServerInfoAsync(key, filter, cancellationToken);

                if (response != null)
                    Cache[filter] = new CacheEntry(response, DateTimeOffset.UtcNow);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Failed to refresh cache for {Filter}", filter);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }, CancellationToken.None);
    }

    private record CacheEntry(GetServerListResponse Value, DateTimeOffset LastUpdate);
}