using System.Linq;
using L4D2PlayStats.Core.Ranking.Results;
using L4D2PlayStats.Sdk.Ranking.Results;
using SdkHistoryResult = L4D2PlayStats.Sdk.Ranking.Results.HistoryResult;

namespace L4D2PlayStats.Web.Models;

public class HomeModel(List<RankingModel> ranking, RankingHistoryResult? lastHistoryResult = null)
{
    private readonly RankingPeriodModel _period = new();

    public int NextResetIn => _period.NextResetIn;
    public List<RankingModel> Ranking { get; } = ranking;
    public RankingHistoryResult? LastHistoryResult { get; } = lastHistoryResult;
    public List<PlayerResult>? LastRankingTopFive => LastHistoryResult?.Players.Take(5).ToList();
    public SdkHistoryResult? LastHistory => LastHistoryResult?.History;
    public bool HasLastRanking => LastRankingTopFive is { Count: > 0 } && LastHistory is not null;
}
