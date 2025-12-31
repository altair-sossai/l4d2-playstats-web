using System.Collections.Concurrent;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using Polly;
using Polly.Retry;

namespace L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;

public class ServerInfoServiceCached(IServerInfoService serverInfoService) : IServerInfoServiceCached
{
    private static readonly ConcurrentDictionary<string, AsyncCache<GetServerListResponse>> ServerInfoCache = new();
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(3, attempt)), (exception, _, retryCount, _) => { Console.WriteLine($"Retry {retryCount} due to: {exception.Message}"); });

    public Task<GetServerListResponse?> GetServerInfoAsync(string key, string filter)
    {
        if (!ServerInfoCache.ContainsKey(filter))
            ServerInfoCache.TryAdd(filter, new AsyncCache<GetServerListResponse>(RefreshInterval));

        var serverInfoCache = ServerInfoCache[filter];

        return serverInfoCache.GetAsync(async () =>
        {
            try
            {
                return await _retryPolicy.ExecuteAsync(async () => await serverInfoService.GetServerInfoAsync(key, filter));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
        });
    }
}