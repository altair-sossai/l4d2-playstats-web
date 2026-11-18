using System.Globalization;
using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Players.Results;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.Steam.SteamUser.Services;
using L4D2PlayStats.Core.UserAvatar;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

namespace L4D2PlayStats.Core.Players.Services;

public class PlayerLookupService(
    IRankingServiceCached rankingService,
    ISteamUserService steamUserService,
    IAppOptionsWraper config,
    IUserAvatar userAvatar,
    IMemoryCache memoryCache) : IPlayerLookupService
{
    private const string AllTimeHistoryId = "all_time";
    private static readonly TimeSpan SteamNameCacheDuration = TimeSpan.FromHours(4);

    public async Task<PlayerLookupResult?> LookupAsync(string? value, CancellationToken cancellationToken = default)
    {
        if (!SteamIdentifiers.TryParse(value?.Trim(), out var identifiers) || identifiers.CommunityId is not > 0)
            return null;

        var communityId = identifiers.CommunityId.Value;
        var siteName = await SiteNameAsync(communityId, cancellationToken);
        var name = siteName ?? await SteamNameAsync(communityId, cancellationToken);

        await userAvatar.LoadAsync(communityId, false, cancellationToken);

        return new PlayerLookupResult
        {
            CommunityId = communityId.ToString(CultureInfo.InvariantCulture),
            SteamId = identifiers.SteamId,
            Steam3 = identifiers.Steam3,
            ProfileUrl = identifiers.ProfileUrl,
            Name = name,
            AvatarUrl = userAvatar[communityId],
            KnownOnSite = siteName != null
        };
    }

    private async Task<string?> SiteNameAsync(long communityId, CancellationToken cancellationToken)
    {
        try
        {
            var current = await rankingService.GetAsync(cancellationToken);
            var player = current.FirstOrDefault(item => item.CommunityId == communityId);

            if (player == null)
            {
                var allTime = await rankingService.HistoryAsync(AllTimeHistoryId, cancellationToken);
                player = allTime.FirstOrDefault(item => item.CommunityId == communityId);
            }

            return string.IsNullOrWhiteSpace(player?.Name) ? null : player.Name;
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            Log.Warning(exception, "Could not read the ranking to look up player {CommunityId}", communityId);

            return null;
        }
    }

    private async Task<string?> SteamNameAsync(long communityId, CancellationToken cancellationToken)
    {
        return await memoryCache.GetOrCreateAsync($"PlayerLookup_SteamName_{communityId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = SteamNameCacheDuration;

            try
            {
                var id = communityId.ToString(CultureInfo.InvariantCulture);
                var response = await steamUserService.GetPlayerSummariesAsync(config.SteamApiKey, id, cancellationToken);
                var player = response?.Response?.Players?.FirstOrDefault();

                return string.IsNullOrWhiteSpace(player?.PersonaName) ? null : player.PersonaName;
            }
            catch (Exception exception) when (exception is not OperationCanceledException)
            {
                Log.Warning(exception, "Could not read the Steam profile of player {CommunityId}", communityId);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);

                return null;
            }
        });
    }
}
