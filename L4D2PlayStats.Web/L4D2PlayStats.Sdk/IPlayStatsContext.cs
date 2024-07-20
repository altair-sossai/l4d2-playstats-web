using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.Sdk.Ranking;
using L4D2PlayStats.Sdk.Statistics;

namespace L4D2PlayStats.Sdk;

public interface IPlayStatsContext
{
    IRankingService RankingService { get; }
    IMatchesService MatchesService { get; }
    IStatisticsService StatisticsService { get; }
}