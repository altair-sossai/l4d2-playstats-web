using L4D2PlayStats.Infrastructure.Extensions;
using L4D2PlayStats.Players;
using L4D2PlayStats.Players.Enums;

namespace L4D2PlayStats.Web.Models;

public class PlayersModel(List<Player> players, PlayerResultProperty orderBy, bool asc)
{
    public List<Player> Players { get; } = players;
    public PlayerResultProperty OrderBy { get; } = orderBy;
    public string OrderByResourceKey { get; } = orderBy.GetResourceKey();
    public bool Asc { get; } = asc;
}