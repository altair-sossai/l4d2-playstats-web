using System.Collections.Concurrent;
using L4D2PlayStats.Core.Infrastructure.Structures;

namespace L4D2PlayStats.Core.Steam.Players.Services.Cache;

public class PlayerServiceCached(IPlayerService playerService) : IPlayerServiceCached
{
    private static readonly ConcurrentDictionary<string, AsyncCache<List<Player>>> PlayersCache = new();
    private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(10);

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
                return await playerService.GetPlayersAsync(ip, port).ToListAsync();
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