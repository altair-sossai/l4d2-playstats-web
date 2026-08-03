using L4D2PlayStats.Core.GameInfo.Results;
using L4D2PlayStats.Sdk.Matches.Results;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PlayerDetailsModel(RankingModel firstPlayerRanking, RankingModel? secondPlayerRanking, List<PlayerResult> players, List<MatchResult> matches, List<PlayerConnectionInfoResult> relatedPlayers)
{
    public RankingModel FirstPlayerRanking { get; } = firstPlayerRanking;
    public RankingModel? SecondPlayerRanking { get; } = secondPlayerRanking;
    public List<PlayerResult> Players { get; } = players;
    public List<PlayerConnectionInfoResult> RelatedPlayers { get; } = relatedPlayers;

    public List<MatchResult> LastMatches { get; } = matches.Where(m => m.Teams != null && m.Teams.Any(t => t.Players != null && t.Players.Any(p => p.CommunityId == firstPlayerRanking.Player.CommunityId.ToString() || (secondPlayerRanking != null && p.CommunityId == secondPlayerRanking.Player.CommunityId.ToString())))).ToList();
}