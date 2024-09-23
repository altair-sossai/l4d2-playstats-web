using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class HistoryModel(HistoryResult history, List<PlayerResult> players, List<HistoryResult> allHistory)
{
    public HistoryResult History { get; } = history;
    public List<PlayerResult> Players { get; } = players;
    public List<HistoryResult> AllHistory { get; } = allHistory;
}