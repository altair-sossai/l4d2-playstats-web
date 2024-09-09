using L4D2PlayStats.Players.Enums;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Players.Extensions;

public static class PlayerResultExtensions
{
    public static IEnumerable<Player> SortPlayers(this IEnumerable<PlayerResult> playersResult, PlayerResultProperty property, bool asc)
    {
        var players = playersResult
            .Select(playerResult => new Player(playerResult, property));

        return asc
            ? players.OrderBy(p => p.Value).ThenBy(p => p.CommunityId)
            : players.OrderByDescending(p => p.Value).ThenByDescending(p => p.CommunityId);
    }
}