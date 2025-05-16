using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.GameInfo.Commands;

public class PlayersCommand
{
    public List<Survivor>? Survivors { get; set; }
    public List<Infected>? Infecteds { get; set; }
    public List<Models.Player>? Spectators { get; set; }
}