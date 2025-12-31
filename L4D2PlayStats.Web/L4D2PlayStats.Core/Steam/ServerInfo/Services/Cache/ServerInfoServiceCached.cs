using System.Collections.Concurrent;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;

namespace L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;

public class ServerInfoServiceCached(IServerInfoService serverInfoService) : IServerInfoServiceCached
{
    private static readonly ConcurrentDictionary<string, AsyncCache<GetServerListResponse>> ServerInfoCache = new();
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

    public Task<GetServerListResponse> GetServerInfoAsync(string key, string filter)
    {
        if (!ServerInfoCache.ContainsKey(filter))
            ServerInfoCache.TryAdd(filter, new AsyncCache<GetServerListResponse>(RefreshInterval));

        var serverInfoCache = ServerInfoCache[filter];

        return serverInfoCache.GetAsync(() => serverInfoService.GetServerInfoAsync(key, filter));
    }
}