using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.Sdk.Matches.Results;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.Matches;

public class MatchesServiceCached(
    IAppOptionsWraper config,
    IMatchesService matchesService,
    IMemoryCache memoryCache) : IMatchesServiceCached
{
    public async Task<List<MatchResult>> GetAsync()
    {
        return (await memoryCache.GetOrCreateAsync("Matches", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            return await matchesService.GetAsync(config.ServerId);
        }))!;
    }
}