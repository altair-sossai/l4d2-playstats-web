using L4D2PlayStats.Core.Players.Enums;

namespace L4D2PlayStats.Web.Models;

public class PlayersModel(List<Core.Players.Player> players, PlayerResultProperty orderBy, bool asc)
{
    public List<Core.Players.Player> Players { get; } = players;
    public PlayerResultProperty OrderBy { get; } = orderBy;
    public bool Asc { get; } = asc;
}