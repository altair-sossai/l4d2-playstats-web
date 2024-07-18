using L4D2PlayStats.Ranking;
using L4D2PlayStats.UserAvatar;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class RankingController(IRankingServiceCached rankingService, IUserAvatar userAvatar) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Ranking = "active";

        var players = await rankingService.GetAsync();
        var communityIds = players.Select(p => p.CommunityId);
        await userAvatar.LoadAsync(communityIds);

        return View(players);
    }
}