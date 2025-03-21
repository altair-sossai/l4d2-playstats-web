using L4D2PlayStats.Sdk.Matches.Results;
using L4D2PlayStats.Sdk.Statistics.Results;

namespace L4D2PlayStats.Web.Models;

public class StatisticsDetailsModel(MatchResult match, List<StatisticsResult> statistics, StatisticsResult? statistic)
{
    public MatchResult Match
    {
        get
        {
            match.ShowDetailsButton = false;
            return match;
        }
    }

    public List<StatisticsResult> Statistics { get; } = statistics;
    public StatisticsResult? Statistic { get; } = statistic;
}