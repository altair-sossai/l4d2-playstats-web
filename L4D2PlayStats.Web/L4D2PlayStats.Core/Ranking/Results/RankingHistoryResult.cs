using L4D2PlayStats.Sdk.Ranking.Results;
using SdkHistoryResult = L4D2PlayStats.Sdk.Ranking.Results.HistoryResult;

namespace L4D2PlayStats.Core.Ranking.Results;

public class RankingHistoryResult
{
    public RankingHistoryResult(SdkHistoryResult? history, List<PlayerResult> players)
    {
        History = history;
        Players = players;
    }

    public SdkHistoryResult? History { get; }

    public List<PlayerResult> Players { get; }
}
