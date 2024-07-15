using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.UserAvatar;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

[Route("last-matches")]
public class LastMatchesController(IConfiguration configuration, IMatchesService matchesService, IUserAvatar userAvatar) : Controller
{
    private string ServerId => configuration.GetValue<string>("ServerId")!;

    public async Task<IActionResult> Index()
    {
        ViewBag.LastMatches = "active";

        var matches = await matchesService.GetAsync(ServerId);

        var communityIds = matches
            .SelectMany(m => m.Teams ?? [])
            .SelectMany(t => t.Players ?? [])
            .Select(p => p.CommunityId);

        await userAvatar.LoadAsync(communityIds);

        return View(matches);
    }
}