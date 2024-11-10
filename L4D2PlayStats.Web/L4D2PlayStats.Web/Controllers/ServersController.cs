using L4D2PlayStats.Steam.Players;
using L4D2PlayStats.Steam.Players.Services;
using L4D2PlayStats.Steam.ServerInfo.Responses;
using L4D2PlayStats.Steam.ServerInfo.Services;
using L4D2PlayStats.UserAvatar;
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

        var model = (await memoryCache.GetOrCreateAsync("Server", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1);

            return GetServerInfoAsync(ServerIp);
        }))!;

        var viewName = model.GameInfo.AnyPlayerConnected ? "_GameInfo" : "_ServersInfo";

        return View(viewName, model);
    }

    private async Task<ServerInfoModel> GetServerInfoAsync(string serverIp)
    {
        var segments = serverIp.Split(':');

        if (segments.Length != 2)
            throw new ArgumentException("Invalid server IP format");

        if (!int.TryParse(segments[1], out var port))
            throw new ArgumentException("Invalid server port");

        var ip = segments[0];
        var gameInfo = GameInfo.GameInfo.GetOrInitializeInstance(userAvatar);
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

    private async Task<List<Player>?> GetPlayersAsync(string ip, int port)
    {
        try
        {
            return await playerService.GetPlayersAsync(ip, port).ToListAsync();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return await Task.FromResult<List<Player>?>(null);
        }
    }
}