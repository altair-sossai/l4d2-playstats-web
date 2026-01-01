using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Core.Ranking.Results;

public class RankingHistoryResult(HistoryResult history, List<PlayerResult> players)
{
    public HistoryResult History { get; } = history;
    public List<PlayerResult> Players { get; } = players;
}