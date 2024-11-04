using L4D2PlayStats.GameInfo.Models;

namespace L4D2PlayStats.GameInfo.Commands;

public class PlayersCommand
{
    public List<Survivor>? Survivors { get; set; }
    public List<Infected>? Infecteds { get; set; }
    public List<Player>? Spectators { get; set; }
}