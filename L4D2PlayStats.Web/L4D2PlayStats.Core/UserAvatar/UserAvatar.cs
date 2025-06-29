﻿using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Steam.SteamUser.Services;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Core.UserAvatar;

public class UserAvatar(ISteamUserService steamUserService, IMemoryCache memoryCache, IAppOptionsWraper config) : IUserAvatar
{
    public string this[long communityId] => this[communityId.ToString()];
    public string this[string? communityId] => memoryCache.Get<string>(communityId ?? string.Empty) ?? "/imgs/avatar-empty.png";

    public Task LoadAsync(params long[] communityIds)
    {
        return LoadAsync(communityIds.AsEnumerable());
    }

    public async Task LoadAsync(IEnumerable<long> communityIds)
    {
        await LoadAsync(communityIds.Select(communityId => communityId.ToString()));
    }

    public async Task LoadAsync(IEnumerable<string?> communityIds)
    {
        var steamIds = new HashSet<string>();

        foreach (var communityId in communityIds)
        {
            if (string.IsNullOrEmpty(communityId))
                continue;

            var avatarUrl = memoryCache.Get<string>(communityId);
            if (!string.IsNullOrEmpty(avatarUrl))
                continue;

            steamIds.Add(communityId);
        }

        if (steamIds.Count == 0)
            return;

        try
        {
            foreach (var steamIdsChunked in steamIds.Chunk(99))
            {
                var response = await steamUserService.GetPlayerSummariesAsync(config.SteamApiKey, string.Join(',', steamIdsChunked));
                if (response?.Response?.Players == null)
                    return;

                foreach (var player in response.Response.Players)
                {
                    if (string.IsNullOrEmpty(player.SteamId) || string.IsNullOrEmpty(player.AvatarFull))
                        continue;

                    using var cacheEntry = memoryCache.CreateEntry(player.SteamId);

                    cacheEntry.Value = player.AvatarFull.Replace("https://", "http://");
                    cacheEntry.AbsoluteExpiration = DateTime.UtcNow.AddHours(4);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}