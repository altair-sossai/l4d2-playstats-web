using L4D2PlayStats.Sdk.Ranking.Results;
using Refit;

namespace L4D2PlayStats.Sdk.Ranking;

public interface IRankingService
{
    [Get("/api/ranking/{serverId}")]
    Task<List<PlayerResult>> GetAsync(string serverId);

    [Get("/api/ranking-config")]
    Task<ExperienceConfigResult> ExperienceConfigAsync();

    [Get("/api/ranking/{serverId}/history")]
    Task<List<HistoryResult>> AllHistoryAsync(string serverId);

    [Get("/api/ranking/{serverId}/history/{historyId}")]
    Task<List<PlayerResult>> HistoryAsync(string serverId, string historyId);
}