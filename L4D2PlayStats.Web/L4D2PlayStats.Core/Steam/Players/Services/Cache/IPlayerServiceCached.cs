namespace L4D2PlayStats.Core.Steam.Players.Services.Cache;

public interface IPlayerServiceCached
{
    Task<List<Player>> GetPlayersAsync(string ip, int port);
}