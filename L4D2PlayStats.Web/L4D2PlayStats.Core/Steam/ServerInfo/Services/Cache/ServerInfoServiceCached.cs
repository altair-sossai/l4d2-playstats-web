using System.Collections.Concurrent;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using Serilog;

namespace L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;

public class ServerInfoServiceCached(IServerInfoService serverInfoService) : IServerInfoServiceCached
{
    private static readonly ConcurrentDictionary<string, AsyncCache<GetServerListResponse>> ServerInfoCache = new();
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(5);

    public Task<GetServerListResponse?> GetServerInfoAsync(string key, string filter, CancellationToken cancellationToken)
    {
        if (!ServerInfoCache.ContainsKey(filter))
            ServerInfoCache.TryAdd(filter, new AsyncCache<GetServerListResponse>(RefreshInterval));

        var serverInfoCache = ServerInfoCache[filter];

        return serverInfoCache.GetAsync(async () =>
        {
            try
            {
                return await serverInfoService.GetServerInfoAsync(key, filter, cancellationToken);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error getting server info");
                return null;
            }
        }, cancellationToken);
    }
}