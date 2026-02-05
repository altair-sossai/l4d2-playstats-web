using L4D2PlayStats.Sdk.Statistics.Commands;
using L4D2PlayStats.Sdk.Statistics.Results;
using Refit;

namespace L4D2PlayStats.Sdk.Statistics;

public interface IStatisticsService
{
    [Get("/api/statistics/{serverId}/{statisticId}")]
    Task<StatisticsResult?> GetStatisticAsync(string serverId, string statisticId, CancellationToken cancellationToken);

    [Get("/api/statistics/{serverId}/between/{start}/and/{end}")]
    Task<List<StatisticsResult>> BetweenAsync(string serverId, string start, string end, CancellationToken cancellationToken);

    [Put("/api/statistics/{statisticId}/update-score")]
    Task<StatisticsResult> UpdateScoreAsync(string statisticId, [Body] UpdateScoreCommand command, CancellationToken cancellationToken);
}