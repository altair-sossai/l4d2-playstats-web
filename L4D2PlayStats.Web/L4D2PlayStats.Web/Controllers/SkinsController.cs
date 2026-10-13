using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.Patents;
using L4D2PlayStats.Core.Patents.Services;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.Skins.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class SkinsController(
    ISkinService skinService,
    IPatentService patentService,
    IRankingServiceCached rankingServiceCached,
    ICurrentUser currentUser) : Controller
{
    [Route("skins")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Skins = "active";

        var playerPatent = await GetPlayerPatentAsync(cancellationToken);

        var model = new SkinsModel(patentService.GetAllPatents(), skinService.GetAllSkins(), playerPatent);

        return View(model);
    }

    private async Task<Patent?> GetPlayerPatentAsync(CancellationToken cancellationToken)
    {
        if (!currentUser.IsAuthenticated)
            return null;

        var players = await rankingServiceCached.GetAsync(cancellationToken);
        var player = players.FirstOrDefault(p => currentUser.ItIsMe(p.CommunityId));

        if (player == null)
            return patentService.GetPatent(0);

        return patentService.GetPatentProgress(player).Patent;
    }
}