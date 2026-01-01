using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Ranking.Results;
using L4D2PlayStats.Sdk.Ranking;
using L4D2PlayStats.Sdk.Ranking.Results;
using Microsoft.Extensions.Caching.Memory;
using SdkHistoryResult = L4D2PlayStats.Sdk.Ranking.Results.HistoryResult;

namespace L4D2PlayStats.Core.Ranking.Services;

public class RankingServiceCached(IAppOptionsWraper config, IMemoryCache memoryCache, IRankingService rankingService) : IRankingServiceCached
{
    public async Task<List<PlayerResult>> GetAsync()
    {
        return (await memoryCache.GetOrCreateAsync("Ranking", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            return await rankingService.GetAsync(config.ServerId);
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

    public async Task<List<SdkHistoryResult>> AllHistoryAsync()
    {
        return (await memoryCache.GetOrCreateAsync("AllHistory", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8);

            return await rankingService.AllHistoryAsync(config.ServerId);
        }))!;
    }

    public async Task<List<PlayerResult>> HistoryAsync(string historyId)
    {
        return (await memoryCache.GetOrCreateAsync($"History_{historyId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8);

            return await rankingService.HistoryAsync(config.ServerId, historyId);
        }))!;
    }

    public async Task<RankingHistoryResult?> LastHistoryAsync()
    {
        var allHistory = await AllHistoryAsync();
        var history = allHistory.LastOrDefault();

        if (history == null)
            return null;

        var historyPlayers = await HistoryAsync(history.Id);
        var players = historyPlayers.ToList();

        return new RankingHistoryResult(history, players);
    }
}
