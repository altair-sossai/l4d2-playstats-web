using L4D2PlayStats.Core.GameInfo;
using L4D2PlayStats.Core.GameInfo.Extensions;
using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class ServersController(
    IAppOptionsWraper config,
    IUserAvatar userAvatar,
    IServerInfoServiceCached serverInfoService)
    : Controller
{
    [Route("servers")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Servers = "active";

        var model = await GetServerInfoAsync(config.PrimaryServerIp, cancellationToken);

        return View(model);
    }

    [Route("servers/header")]
    public async Task<IActionResult> Header(CancellationToken cancellationToken)
    {
        var model = await GetServerInfoAsync(config.PrimaryServerIp, cancellationToken);

        return PartialView("_Header", model);
    }

    [Route("servers/players")]
    public async Task<IActionResult> Players(CancellationToken cancellationToken)
    {
        var model = await GetServerInfoAsync(config.PrimaryServerIp, cancellationToken);

        return PartialView("_Players", model);
    }

    [Route("servers/messages")]
    public async Task<IActionResult> Messages([FromQuery] long after = 0, CancellationToken cancellationToken = default)
    {
        var model = await GetServerInfoAsync(config.PrimaryServerIp, cancellationToken);

        var messages = model.GameInfo.AllMessages
            .After(after)
            .ToList();

        if (messages.Count == 0)
            return NoContent();

        return PartialView("_Messages", messages);
    }

    private async Task<ServerInfoModel> GetServerInfoAsync(string serverIp, CancellationToken cancellationToken)
    {
        var segments = serverIp.Split(':');

        if (segments.Length != 2)
            throw new ArgumentException("Invalid server IP format");

        if (!int.TryParse(segments[1], out var port))
            throw new ArgumentException("Invalid server port");

        var ip = segments[0];
        var gameInfo = GameInfo.GetOrInitializeInstance(userAvatar);
        var serverInfo = await serverInfoService.GetServerInfoAsync(config.SteamApiKey, $"addr\\{ip}:{port}", cancellationToken);

        return new ServerInfoModel(serverIp, gameInfo, serverInfo);
    }
}