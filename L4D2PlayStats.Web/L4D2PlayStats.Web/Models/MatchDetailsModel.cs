using L4D2PlayStats.Sdk.Matches.Results;
using L4D2PlayStats.Sdk.Statistics.Results;

namespace L4D2PlayStats.Web.Models;

public class MatchDetailsModel
{
    private readonly MatchResult? _match;

    public MatchResult? Match
    {
        get => _match;
        init
        {
            _match = value;

            if (value != null)
                value.ShowDetailsButton = false;
        }
    }

    public List<StatisticsResult>? Statistics { get; set; }
}