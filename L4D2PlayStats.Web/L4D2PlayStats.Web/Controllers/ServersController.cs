using L4D2PlayStats.Core.GameInfo;
using L4D2PlayStats.Core.GameInfo.Extensions;
using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Steam.Players.Services;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using L4D2PlayStats.Core.Steam.ServerInfo.Services;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Web.Controllers;

public class ServersController(
    IAppOptionsWraper config,
    IMemoryCache memoryCache,
    IUserAvatar userAvatar,
    IServerInfoService serverInfoService,
    IPlayerService playerService)
    : Controller
{
    private static readonly AsyncCache<List<Core.Steam.Players.Player>?> PlayersCache = new(TimeSpan.FromSeconds(10));
    private static readonly AsyncCache<GetServerListResponse?> ServerInfoCache = new(TimeSpan.FromSeconds(10));

    [Route("servers")]
    public async Task<IActionResult> Index()
    {
        ViewBag.Servers = "active";

        var model = await GetServerInfoCachedAsync(config.PrimaryServerIp);
        var viewName = model.GameInfo.AnyPlayerConnected ? "_GameInfo" : "_ServersInfo";

        return View(viewName, model);
    }

    [Route("servers/header")]
    public async Task<IActionResult> Header()
    {
        var model = await GetServerInfoCachedAsync(config.PrimaryServerIp);

        return PartialView("_Header", model);
    }

    [Route("servers/players")]
    public async Task<IActionResult> Players()
    {
        var model = await GetServerInfoCachedAsync(config.PrimaryServerIp);

        return PartialView("_Players", model);
    }

    [Route("servers/messages")]
    public async Task<IActionResult> Messages([FromQuery] long after = 0)
    {
        var model = await GetServerInfoCachedAsync(config.PrimaryServerIp);

        var messages = model.GameInfo.AllMessages
            .After(after)
            .ToList();

        return PartialView("_Messages", messages);
    }

    private Task<ServerInfoModel> GetServerInfoCachedAsync(string serverIp)
    {
        return memoryCache.GetOrCreateAsync($"Server:{serverIp}", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);

            return GetServerInfoAsync(serverIp);
        })!;
    }

    private async Task<ServerInfoModel> GetServerInfoAsync(string serverIp)
    {
        var segments = serverIp.Split(':');

        if (segments.Length != 2)
            throw new ArgumentException("Invalid server IP format");

        if (!int.TryParse(segments[1], out var port))
            throw new ArgumentException("Invalid server port");

        var ip = segments[0];
        var gameInfo = GameInfo.GetOrInitializeInstance(userAvatar);
        var serverInfo = await GetServerInfoCacheAsync(ip, port);
        var players = await GetPlayersCacheAsync(ip, port);

        return new ServerInfoModel(serverIp, gameInfo, serverInfo, players);
    }

    private Task<GetServerListResponse?> GetServerInfoCacheAsync(string ip, int port)
    {
        return ServerInfoCache.GetAsync(() => serverInfoService.GetServerInfo(config.SteamApiKey, $"addr\\{ip}:{port}")!);
    }

    private Task<List<Core.Steam.Players.Player>?> GetPlayersCacheAsync(string ip, int port)
    {
        return PlayersCache.GetAsync(async () => await playerService.GetPlayersAsync(ip, port).ToListAsync());
    }
}