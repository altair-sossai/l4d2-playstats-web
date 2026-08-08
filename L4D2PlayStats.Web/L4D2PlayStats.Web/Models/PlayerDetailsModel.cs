using L4D2PlayStats.Core.GameInfo.Results;
using L4D2PlayStats.Sdk.Matches.Results;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PlayerDetailsModel(
    RankingModel firstPlayerRanking,
    RankingModel? secondPlayerRanking,
    List<PlayerResult> players,
    List<MatchResult> matches,
    List<PlayerConnectionInfoResult> relatedPlayers
)
{
    public RankingModel FirstPlayerRanking { get; } = firstPlayerRanking;
    public RankingModel? SecondPlayerRanking { get; } = secondPlayerRanking;
    public List<PlayerResult> Players { get; } = players;
    public List<PlayerConnectionInfoResult> RelatedPlayers { get; } = relatedPlayers;

    public List<MatchResult> LastMatches { get; } =
    [
        .. matches
            .Where(m => m.Teams != null && m.Teams
                .Any(t => t.Players != null && t.Players
                    .Any(p => p.CommunityId == firstPlayerRanking.Player.CommunityId.ToString()
                              || (secondPlayerRanking != null && p.CommunityId == secondPlayerRanking.Player.CommunityId.ToString())
                    )
                )
            )
    ];

    private Dictionary<string, PlayerRelationResult> FirstPlayerRelations => FirstPlayerRanking.Player.Relations ?? [];
    private Dictionary<string, PlayerRelationResult> SecondPlayerRelations => SecondPlayerRanking?.Player.Relations ?? [];

    private List<PlayerRelationRowModel> RelationRows => field ??= BuildRelationRows();

    public PlayerRelationModel? HeadToHead
    {
        get
        {
            if (SecondPlayerRanking == null)
                return null;

            var relation = FirstPlayerRelations.GetValueOrDefault(SecondPlayerRanking.Player.CommunityId.ToString());

            return relation == null ? null : new PlayerRelationModel(relation, SecondPlayerRanking.Player);
        }
    }

    public List<PlayerRelationRowModel> Relations
    {
        get
        {
            var secondPlayerId = SecondPlayerRanking?.Player.CommunityId;

            return
            [
                .. RelationRows
                    .Where(row => row.TogetherGames > 0 || row.AgainstGames > 0)
                    .OrderByDescending(row => row.CommunityId == secondPlayerId)
                    .ThenByDescending(row => row.TogetherGames + row.AgainstGames)
                    .ThenByDescending(row => row.TogetherWins + row.AgainstWins)
            ];
        }
    }

    private List<PlayerRelationRowModel> BuildRelationRows()
    {
        var firstPlayerId = FirstPlayerRanking.Player.CommunityId;
        var secondPlayerId = SecondPlayerRanking?.Player.CommunityId;

        var counterpartIds = FirstPlayerRelations.Values
            .Concat(SecondPlayerRelations.Values)
            .Select(relation => relation.CommunityId)
            .Where(communityId => communityId != firstPlayerId)
            .Distinct();

        var rows = new List<PlayerRelationRowModel>();

        foreach (var communityId in counterpartIds)
        {
            var counterpart = Players.FirstOrDefault(player => player.CommunityId == communityId);
            var first = FirstPlayerRelations.GetValueOrDefault(communityId.ToString());

            var secondCounterpartId = communityId == secondPlayerId ? firstPlayerId : communityId;
            var second = SecondPlayerRelations.GetValueOrDefault(secondCounterpartId.ToString());

            rows.Add(new PlayerRelationRowModel(communityId, counterpart, first, second));
        }

        return rows;
    }
}