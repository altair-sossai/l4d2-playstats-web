using L4D2PlayStats.Core.GameInfo;
using L4D2PlayStats.Core.Steam.Players.Services;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using L4D2PlayStats.Core.Steam.ServerInfo.Services;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Web.Controllers;

public class ServersController(
    IConfiguration configuration,
    IMemoryCache memoryCache,
    IUserAvatar userAvatar,
    IServerInfoService serverInfoService,
    IPlayerService playerService)
    : Controller
{
    private string[] ServerIPs => configuration.GetValue<string>("ServerIPs")?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];
    private string ServerIp => ServerIPs.First();
    private string SteamApiKey => configuration.GetValue<string>("SteamApiKey")!;

    [Route("servers")]
    public async Task<IActionResult> Index()
    {
        ViewBag.Servers = "active";

        var model = await GetServerInfoCacheedAsync(ServerIp);
        var viewName = model.GameInfo.AnyPlayerConnected ? "_GameInfo" : "_ServersInfo";

        return View(viewName, model);
    }

    [Route("servers/header")]
    public async Task<IActionResult> Header()
    {
        var model = await GetServerInfoCacheedAsync(ServerIp);

        return PartialView("_Header", model);
    }

    [Route("servers/players")]
    public async Task<IActionResult> Players()
    {
        var model = await GetServerInfoCacheedAsync(ServerIp);

        return PartialView("_Players", model);
    }

    [Route("servers/messages")]
    public async Task<IActionResult> Messages([FromQuery] int after = 0)
    {
        var model = await GetServerInfoCacheedAsync(ServerIp);
        var messages = model.GameInfo.Messages.Where(m => m.When > after).ToList();

        return PartialView("_Messages", messages);
    }

    private Task<ServerInfoModel> GetServerInfoCacheedAsync(string serverIp)
    {
        return memoryCache.GetOrCreateAsync("Server", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);

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
        var serverInfo = await GetServerInfo(ip, port);
        var players = await GetPlayersAsync(ip, port);

        return new ServerInfoModel(serverIp, gameInfo, serverInfo, players);
    }

    private async Task<GetServerListResponse?> GetServerInfo(string ip, int port)
    {
        try
        {
            return await serverInfoService.GetServerInfo(SteamApiKey, $"addr\\{ip}:{port}");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return await Task.FromResult<GetServerListResponse?>(null);
        }
    }

    private async Task<List<Core.Steam.Players.Player>?> GetPlayersAsync(string ip, int port)
    {
        try
        {
            return await playerService.GetPlayersAsync(ip, port).ToListAsync();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return await Task.FromResult<List<Core.Steam.Players.Player>?>(null);
        }
    }
}