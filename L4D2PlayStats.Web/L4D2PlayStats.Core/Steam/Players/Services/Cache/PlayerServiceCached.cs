using System.Collections.Concurrent;
using L4D2PlayStats.Core.Infrastructure.Structures;
using Polly;
using Polly.Retry;

namespace L4D2PlayStats.Core.Steam.Players.Services.Cache;

public class PlayerServiceCached(IPlayerService playerService) : IPlayerServiceCached
{
    private static readonly ConcurrentDictionary<string, AsyncCache<List<Player>>> PlayersCache = new();
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(3, attempt)), (exception, _, retryCount, _) => { Console.WriteLine($"Retry {retryCount} due to: {exception.Message}"); });

    public async Task<List<Player>> GetPlayersAsync(string ip, int port)
    {
        var key = $"{ip}:{port}";

        if (!PlayersCache.ContainsKey(key))
            PlayersCache.TryAdd(key, new AsyncCache<List<Player>>(RefreshInterval));

        var playersCache = PlayersCache[key];

        var players = await playersCache.GetAsync(async () =>
        {
            try
            {
                return await _retryPolicy.ExecuteAsync(async () => await playerService.GetPlayersAsync(ip, port).ToListAsync());
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }
        });

        return players ?? [];
    }
}