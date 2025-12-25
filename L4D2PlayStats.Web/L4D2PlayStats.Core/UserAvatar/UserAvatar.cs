using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Steam.SteamUser.Services;
using Microsoft.AspNetCore.Hosting;

namespace L4D2PlayStats.Core.UserAvatar;

public class UserAvatar(ISteamUserService steamUserService, IAppOptionsWraper config, IWebHostEnvironment env) : IUserAvatar
{
    private const string EmptyAvatarPath = "/imgs/avatar-empty.png";
    private static readonly SemaphoreSlim SemaphoreSlim = new(5);
    private static readonly HttpClient HttpClient = new();

    public string this[long communityId] => this[communityId.ToString()];

    public string this[string? communityId]
    {
        get
        {
            if (string.IsNullOrEmpty(communityId))
                return EmptyAvatarPath;

            var fileInfo = FileInfo(communityId);

            return fileInfo.Exists ? RelativeUrl(communityId) : EmptyAvatarPath;
        }
    }

    public Task LoadAsync(long communityId, bool fireAndForget = true)
    {
        return LoadAsync([communityId], fireAndForget);
    }

    public async Task LoadAsync(IEnumerable<long> communityIds, bool fireAndForget = true)
    {
        await LoadAsync(communityIds.Select(communityId => communityId.ToString()), fireAndForget);
    }

    public async Task LoadAsync(IEnumerable<string?> communityIds, bool fireAndForget = true)
    {
        var steamIds = communityIds
            .Where(NeedToDownload)
            .Cast<string>()
            .ToList();

        if (steamIds.Count == 0)
            return;

        var tasks = new List<Task>
        {
            DownloadSteamPlayerAvatarsAsync(steamIds)
        };

        if (fireAndForget)
            tasks.Add(Task.Delay(TimeSpan.FromSeconds(1)));

        await Task.WhenAny(tasks);
    }

    private async Task DownloadSteamPlayerAvatarsAsync(IEnumerable<string> steamIds)
    {
        foreach (var steamIdsChunked in steamIds.Chunk(99))
        {
            var response = await steamUserService.GetPlayerSummariesAsync(config.SteamApiKey, string.Join(',', steamIdsChunked));

            if (response?.Response?.Players == null)
                continue;

            var tasks = response.Response.Players
                .Select(async player =>
                {
                    if (string.IsNullOrEmpty(player.SteamId) || string.IsNullOrEmpty(player.AvatarFull))
                        return;

                    await SemaphoreSlim.WaitAsync();

                    try
                    {
                        var fileInfo = FileInfo(player.SteamId);

                        if (fileInfo.Directory is { Exists: false })
                            fileInfo.Directory.Create();

                        await using var stream = await HttpClient.GetStreamAsync(player.AvatarFull);
                        await using var fileStream = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);

                        await stream.CopyToAsync(fileStream);
                    }
                    finally
                    {
                        SemaphoreSlim.Release();
                    }
                });

            await Task.WhenAll(tasks);
        }
    }

    private bool NeedToDownload(string? communityId)
    {
        if (string.IsNullOrEmpty(communityId))
            return false;

        var fileInfo = FileInfo(communityId);

        if (!fileInfo.Exists)
            return true;

        var diff = DateTime.UtcNow - fileInfo.LastWriteTimeUtc;

        return diff.TotalHours > 24;
    }

    private FileInfo FileInfo(string communityId)
    {
        var path = Path.Combine(env.WebRootPath, "imgs", "players", $"{communityId}.jpg".ToLowerInvariant());

        return new FileInfo(path);
    }

    private static string RelativeUrl(string communityId)
    {
        return $"/imgs/players/{communityId.ToLowerInvariant()}.jpg";
    }
}