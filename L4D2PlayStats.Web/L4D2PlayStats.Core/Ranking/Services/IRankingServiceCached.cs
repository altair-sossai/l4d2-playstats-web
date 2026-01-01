using L4D2PlayStats.Core.Ranking.Results;
using L4D2PlayStats.Sdk.Ranking.Results;
using SdkHistoryResult = L4D2PlayStats.Sdk.Ranking.Results.HistoryResult;

namespace L4D2PlayStats.Core.Ranking.Services;

public interface IRankingServiceCached
{
    Task<List<PlayerResult>> GetAsync();
    Task<ExperienceConfigResult> ExperienceConfigAsync();
    Task<List<SdkHistoryResult>> AllHistoryAsync();
    Task<List<PlayerResult>> HistoryAsync(string historyId);
    Task<HistoryResult?> LastHistoryAsync();
}
