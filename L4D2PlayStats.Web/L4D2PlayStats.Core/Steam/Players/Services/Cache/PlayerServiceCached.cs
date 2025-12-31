using System.Collections.Concurrent;
using L4D2PlayStats.Core.Infrastructure.Structures;

namespace L4D2PlayStats.Core.Steam.Players.Services.Cache;

public class PlayerServiceCached(IPlayerService playerService) : IPlayerServiceCached
{
    private static readonly ConcurrentDictionary<string, AsyncCache<List<Player>>> PlayersCache = new();
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

    public Task<List<Player>> GetPlayersAsync(string ip, int port)
    {
        var key = $"{ip}:{port}";

        if (!PlayersCache.ContainsKey(key))
            PlayersCache.TryAdd(key, new AsyncCache<List<Player>>(RefreshInterval));

        var playersCache = PlayersCache[key];

        return playersCache.GetAsync(async () => await playerService.GetPlayersAsync(ip, port).ToListAsync());
    }
}