using L4D2PlayStats.Core.Ranking.Results;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Core.Ranking.Services;

public interface IRankingServiceCached
{
    Task<List<PlayerResult>> GetAsync(CancellationToken cancellationToken);
    Task<ExperienceConfigResult> ExperienceConfigAsync(CancellationToken cancellationToken);
    Task<List<HistoryResult>> AllHistoryAsync(CancellationToken cancellationToken);
    Task<List<PlayerResult>> HistoryAsync(string historyId, CancellationToken cancellationToken);
    Task<RankingHistoryResult?> LastHistoryAsync(CancellationToken cancellationToken);
}