using L4D2PlayStats.Sdk.Matches.Results;

namespace L4D2PlayStats.Matches;

public interface IMatchesServiceCached
{
    Task<List<MatchResult>> GetAsync();
}