using System.Linq;
using L4D2PlayStats.Sdk.Ranking.Results;
using SdkHistoryResult = L4D2PlayStats.Sdk.Ranking.Results.HistoryResult;

namespace L4D2PlayStats.Core.Ranking.Results;

public class HistoryResult
{
    public HistoryResult(SdkHistoryResult? history, List<PlayerResult> players)
    {
        History = history;
        Players = players;
    }

    public SdkHistoryResult? History { get; }

    public List<PlayerResult> Players { get; }

    public IReadOnlyList<PlayerResult> TopFivePlayers => Players.Take(5).ToList();
}
