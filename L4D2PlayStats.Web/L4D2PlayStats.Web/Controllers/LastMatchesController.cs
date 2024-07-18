using L4D2PlayStats.Matches;
using L4D2PlayStats.UserAvatar;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

[Route("last-matches")]
public class LastMatchesController(IMatchesServiceCached matchesService, IUserAvatar userAvatar) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.LastMatches = "active";

        var matches = await matchesService.GetAsync();

        var communityIds = matches
            .SelectMany(m => m.Teams ?? [])
            .SelectMany(t => t.Players ?? [])
            .Select(p => p.CommunityId);

        await userAvatar.LoadAsync(communityIds);

        return View(matches);
    }
}