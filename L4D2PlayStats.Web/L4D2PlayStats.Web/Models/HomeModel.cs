using L4D2PlayStats.Core.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class HomeModel(List<RankingModel> ranking, RankingHistoryResult? lastHistory = null)
{
    private readonly RankingPeriodModel _period = new();

    public int NextResetIn => _period.NextResetIn;
    public List<RankingModel> Ranking { get; } = ranking;
    public RankingHistoryResult? LastHistory { get; } = lastHistory;
}