using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class HomeModel(List<RankingModel> ranking, List<PlayerResult>? lastRankingTopFive, HistoryResult? lastHistory)
{
    private readonly RankingPeriodModel _period = new();

    public int NextResetIn => _period.NextResetIn;
    public List<RankingModel> Ranking { get; } = ranking;
    public List<PlayerResult>? LastRankingTopFive { get; } = lastRankingTopFive;
    public HistoryResult? LastHistory { get; } = lastHistory;
    public bool HasLastRanking => LastRankingTopFive is { Count: > 0 } && LastHistory is not null;
}
