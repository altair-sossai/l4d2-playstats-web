using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PlayerDetailsModel(RankingModel firstPlayerRanking, RankingModel? secondPlayerRanking, List<PlayerResult> players)
{
    public RankingModel FirstPlayerRanking { get; } = firstPlayerRanking;
    public RankingModel? SecondPlayerRanking { get; } = secondPlayerRanking;
    public List<PlayerResult> Players { get; } = players;
}