using L4D2PlayStats.Core.Infrastructure.Extensions;
using L4D2PlayStats.Core.Players.Enums;

namespace L4D2PlayStats.Web.Models;

public class PlayersModel(List<Core.Players.Player> players, PlayerResultProperty orderBy, bool asc)
{
    public List<Core.Players.Player> Players { get; } = players;
    public PlayerResultProperty OrderBy { get; } = orderBy;
    public string OrderByResourceKey { get; } = orderBy.GetResourceKey();
    public bool Asc { get; } = asc;
}