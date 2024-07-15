using L4D2PlayStats.Sdk.Ranking;
using L4D2PlayStats.UserAvatar;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class RankingController(IConfiguration configuration, IRankingService rankingService, IUserAvatar userAvatar) : Controller
{
    private string ServerId => configuration.GetValue<string>("ServerId")!;

    public async Task<IActionResult> Index()
    {
        ViewBag.Ranking = "active";

        var players = await rankingService.GetAsync(ServerId);
        var communityIds = players.Select(p => p.CommunityId);
        await userAvatar.LoadAsync(communityIds);

        return View(players);
    }
}