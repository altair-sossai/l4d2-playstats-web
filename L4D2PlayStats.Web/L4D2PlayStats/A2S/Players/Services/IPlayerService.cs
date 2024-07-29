namespace L4D2PlayStats.A2S.Players.Services;

public interface IPlayerService
{
    IAsyncEnumerable<Player> GetPlayersAsync(string ip, int port);
}