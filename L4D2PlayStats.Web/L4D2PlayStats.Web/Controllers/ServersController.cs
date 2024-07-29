using L4D2PlayStats.A2S.Players.Services;
using L4D2PlayStats.A2S.Servers.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Web.Controllers;

public class ServersController(
    IConfiguration configuration,
    IMemoryCache memoryCache,
    IServerService serverService,
    IPlayerService playerService) : Controller
{
    private string[] ServerIPs => configuration
        .GetValue<string>("ServerIPs")?
        .Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];

    public async Task<IActionResult> Index()
    {
        ViewBag.Servers = "active";

        var models = (await memoryCache.GetOrCreateAsync("Servers", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            var servers = new List<ServerModel>();

            foreach (var serverIp in ServerIPs)
            {
                var segments = serverIp.Split(':');

                if (segments.Length != 2)
                    throw new ArgumentException("Invalid server IP format");

                if (!int.TryParse(segments[1], out var port))
                    throw new ArgumentException("Invalid server port");

                var ip = segments[0];
                var serverInfo = await serverService.GetServerInfoAsync(ip, port);
                var players = await playerService.GetPlayersAsync(ip, port).ToListAsync();
                var server = new ServerModel(serverIp, serverInfo, players);

                servers.Add(server);
            }

            return servers;
        }))!;

        return View(models);
    }
}