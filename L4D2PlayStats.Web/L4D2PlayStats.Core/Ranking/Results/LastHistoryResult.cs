using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Core.Ranking.Results;

public class LastHistoryResult
{
    public LastHistoryResult(HistoryResult? history, List<PlayerResult> players)
    {
        History = history;
        Players = players;
    }

    public HistoryResult? History { get; }

    public List<PlayerResult> Players { get; }
}
