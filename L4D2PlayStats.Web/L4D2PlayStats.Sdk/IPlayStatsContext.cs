using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.Sdk.Ranking;

namespace L4D2PlayStats.Sdk;

public interface IPlayStatsContext
{
    IRankingService RankingService { get; }
    IMatchesService MatchesService { get; }
}