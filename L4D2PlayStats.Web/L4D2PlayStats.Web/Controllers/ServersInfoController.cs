using L4D2PlayStats.A2S.Players;
using L4D2PlayStats.A2S.Players.Services;
using L4D2PlayStats.A2S.Servers;
using L4D2PlayStats.A2S.Servers.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Web.Controllers;

public class ServersInfoController(
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
        ViewBag.ServersInfo = "active";

        var models = (await memoryCache.GetOrCreateAsync("Servers", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            var servers = new List<ServerModel>();

            foreach (var serverIp in ServerIPs)
                servers.Add(await GetServerAsync(serverIp));

            return servers;
        }))!;

        return View(models);
    }

    private async Task<ServerModel> GetServerAsync(string serverIp)
    {
        var segments = serverIp.Split(':');

        if (segments.Length != 2)
            throw new ArgumentException("Invalid server IP format");

        if (!int.TryParse(segments[1], out var port))
            throw new ArgumentException("Invalid server port");

        var ip = segments[0];

        ServerInfo? serverInfo = null;
        List<Player>? players = null;

        try
        {
            serverInfo = await serverService.GetServerInfoAsync(ip, port);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        try
        {
            players = await playerService.GetPlayersAsync(ip, port).ToListAsync();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }

        return new ServerModel(serverIp, serverInfo, players);
    }
}