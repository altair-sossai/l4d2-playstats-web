using System.Linq;
using L4D2PlayStats.Core.Ranking.Results;
using L4D2PlayStats.Sdk.Ranking.Results;
using SdkHistoryResult = L4D2PlayStats.Sdk.Ranking.Results.HistoryResult;

namespace L4D2PlayStats.Web.Models;

public class HomeModel(List<RankingModel> ranking, HistoryResult? lastHistoryResult)
{
    private readonly RankingPeriodModel _period = new();

    public int NextResetIn => _period.NextResetIn;
    public List<RankingModel> Ranking { get; } = ranking;
    public HistoryResult? LastHistoryResult { get; } = lastHistoryResult;
    public List<PlayerResult>? LastRankingTopFive => LastHistoryResult?.TopFivePlayers.ToList();
    public SdkHistoryResult? LastHistory => LastHistoryResult?.History;
    public bool HasLastRanking => LastRankingTopFive is { Count: > 0 } && LastHistory is not null;
}
