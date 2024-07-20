using L4D2PlayStats.Sdk.Statistics.Results;
using Refit;

namespace L4D2PlayStats.Sdk.Statistics;

public interface IStatisticsService
{
    [Get("/api/statistics/{serverId}/between/{start}/and/{end}")]
    Task<List<StatisticsResult>> BetweenAsync(string serverId, string start, string end);
}