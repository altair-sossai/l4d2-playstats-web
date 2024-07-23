using L4D2PlayStats.Sdk.Matches.Results;
using L4D2PlayStats.Sdk.Statistics.Results;

namespace L4D2PlayStats.Web.Models;

public class MatchDetailsModel(MatchResult match, List<StatisticsResult> statistics)
{
    public MatchResult Match
    {
        get
        {
            match.ShowDetailsButton = false;
            return match;
        }
    }

    public List<StatisticsResult> Statistics { get; set; } = statistics;
}