using L4D2PlayStats.Sdk.Ranking;
using L4D2PlayStats.Sdk.Ranking.Results;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace L4D2PlayStats.Core.Ranking.Services;

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

    public async Task<List<HistoryResult>> AllHistoryAsync()
    {
        return (await memoryCache.GetOrCreateAsync("AllHistory", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8);

            return await rankingService.AllHistoryAsync(ServerId);
        }))!;
    }

    public async Task<List<PlayerResult>> HistoryAsync(string historyId)
    {
        return (await memoryCache.GetOrCreateAsync($"History_{historyId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8);

            return await rankingService.HistoryAsync(ServerId, historyId);
        }))!;
    }
}