using L4D2PlayStats.Sdk.Ranking.Results;
using Refit;

namespace L4D2PlayStats.Sdk.Ranking;

public interface IRankingService
{
    [Get("/api/ranking/{serverId}")]
    Task<List<PlayerResult>> GetAsync(string serverId);
}