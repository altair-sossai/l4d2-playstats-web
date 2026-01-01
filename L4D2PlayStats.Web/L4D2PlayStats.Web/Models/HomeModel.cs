using L4D2PlayStats.Core.Ranking.Results;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class HomeModel(List<RankingModel> ranking, LastHistoryResult? lastHistoryResult)
{
    private readonly RankingPeriodModel _period = new();

    public int NextResetIn => _period.NextResetIn;
    public List<RankingModel> Ranking { get; } = ranking;
    public LastHistoryResult? LastHistoryResult { get; } = lastHistoryResult;
    public List<PlayerResult>? LastRankingTopFive => LastHistoryResult?.Players;
    public HistoryResult? LastHistory => LastHistoryResult?.History;
    public bool HasLastRanking => LastRankingTopFive is { Count: > 0 } && LastHistory is not null;
}
