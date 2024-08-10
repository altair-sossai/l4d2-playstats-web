using L4D2PlayStats.Steam.Players.Services;
using L4D2PlayStats.Steam.ServerInfo.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Web.Controllers;

public class ServersController(
    IConfiguration configuration,
    IMemoryCache memoryCache,
    IServerInfoService serverInfoService,
    IPlayerService playerService) : Controller
{
    private string[] ServerIPs => configuration.GetValue<string>("ServerIPs")?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];
    private string SteamApiKey => configuration.GetValue<string>("SteamApiKey")!;

    public async Task<IActionResult> Index()
    {
        ViewBag.Servers = "active";

        var models = (await memoryCache.GetOrCreateAsync("Servers", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            var servers = new List<ServerInfoModel>();

            foreach (var serverIp in ServerIPs)
                servers.Add(await GetServerInfoAsync(serverIp));

            return servers;
        }))!;

        return View(models);
    }

    private async Task<ServerInfoModel> GetServerInfoAsync(string serverIp)
    {
        var segments = serverIp.Split(':');

        if (segments.Length != 2)
            throw new ArgumentException("Invalid server IP format");

        if (!int.TryParse(segments[1], out var port))
            throw new ArgumentException("Invalid server port");

        var ip = segments[0];
        var serverInfoResponse = await serverInfoService.GetServerInfo(SteamApiKey, $"addr\\{ip}:{port}");
        var players = await playerService.GetPlayersAsync(ip, port).ToListAsync();

        return new ServerInfoModel(serverIp, serverInfoResponse, players);
    }
}