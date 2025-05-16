namespace L4D2PlayStats.Core.Steam.Players.Services;

public interface IPlayerService
{
    IAsyncEnumerable<Player> GetPlayersAsync(string ip, int port);
}