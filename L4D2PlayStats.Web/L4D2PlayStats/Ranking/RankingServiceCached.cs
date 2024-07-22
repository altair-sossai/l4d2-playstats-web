using L4D2PlayStats.Sdk.Ranking;
using L4D2PlayStats.Sdk.Ranking.Results;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Ranking;

public class RankingServiceCached(IConfiguration configuration, IMemoryCache memoryCache, IRankingService rankingService) : IRankingServiceCached
{
    private string ServerId => configuration.GetValue<string>("ServerId")!;

    public async Task<List<PlayerResult>> GetAsync()
    {
        return (await memoryCache.GetOrCreateAsync("Ranking", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            return await rankingService.GetAsync(ServerId);
        }))!;
    }

    public async Task<ExperienceConfigResult> ExperienceConfigAsync()
    {
        return (await memoryCache.GetOrCreateAsync("ExperienceConfig", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            return await rankingService.ExperienceConfigAsync();
        }))!;
    }
}