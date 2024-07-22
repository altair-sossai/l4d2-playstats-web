using L4D2PlayStats.Sdk.Matches.Results;
using L4D2PlayStats.Sdk.Statistics.Results;

namespace L4D2PlayStats.Web.Models;

public class MatchDetailsModel(MatchResult match, List<StatisticsResult> statistics)
{
    public MatchResult Match
    {
        get => match;
        init
        {
            match = value;
            match.ShowDetailsButton = false;
        }
    }

    public List<StatisticsResult> Statistics { get; set; } = statistics;
}