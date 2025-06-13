using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.Sdk.Matches.Results;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace L4D2PlayStats.Core.Matches;

public class MatchesServiceCached(
    IOptions<AppOptions> config,
    IMatchesService matchesService,
    IMemoryCache memoryCache) : IMatchesServiceCached
{
    private string ServerId => config.Value.ServerId!;

    public async Task<List<MatchResult>> GetAsync()
    {
        return (await memoryCache.GetOrCreateAsync("Matches", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            return await matchesService.GetAsync(ServerId);
        }))!;
    }
}