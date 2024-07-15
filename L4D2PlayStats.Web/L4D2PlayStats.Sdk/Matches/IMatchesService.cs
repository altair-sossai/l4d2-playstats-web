using L4D2PlayStats.Sdk.Matches.Results;
using Refit;

namespace L4D2PlayStats.Sdk.Matches;

public interface IMatchesService
{
    [Get("/api/matches/{serverId}")]
    Task<List<MatchResult>> GetAsync(string serverId, int count = 15);
}