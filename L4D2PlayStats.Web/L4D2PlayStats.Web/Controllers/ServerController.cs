using L4D2PlayStats.Core.GameInfo;
using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class ServerController(
    IAppOptionsWraper config,
    IUserAvatar userAvatar,
    IServerInfoServiceCached serverInfoService)
    : Controller
{
    [Route("server")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Server = "active";

        var model = await GetServerInfoAsync(cancellationToken);

        return View(model);
    }

    [Route("servers")]
    public IActionResult IndexLegacy()
    {
        return RedirectToActionPermanent(nameof(Index));
    }

    [Route("server/header")]
    public async Task<IActionResult> Header(CancellationToken cancellationToken)
    {
        var model = await GetServerInfoAsync(cancellationToken);

        return PartialView("_Header", model);
    }

    [Route("server/players")]
    public async Task<IActionResult> Players(CancellationToken cancellationToken)
    {
        var model = await GetServerInfoAsync(cancellationToken);

        return PartialView("_Players", model);
    }

    [Route("server/feed")]
    public IActionResult Feed([FromQuery] long after = 0)
    {
        var gameInfo = GameInfo.GetOrInitializeInstance(userAvatar);
        var feed = gameInfo.After(after);

        if (feed.Count == 0)
            return NoContent();

        return PartialView("_Feed", feed);
    }

    private async Task<ServerInfoModel> GetServerInfoAsync(CancellationToken cancellationToken)
    {
        var gameInfo = GameInfo.GetOrInitializeInstance(userAvatar);
        var serverInfo = await serverInfoService.GetServerInfoAsync(cancellationToken);

        return new ServerInfoModel(config.ServerIp, config.ServerDns, gameInfo, serverInfo);
    }
}