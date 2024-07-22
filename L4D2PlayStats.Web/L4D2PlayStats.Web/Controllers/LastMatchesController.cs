using L4D2PlayStats.Matches;
using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.Sdk.Statistics;
using L4D2PlayStats.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

[Route("last-matches")]
public class LastMatchesController(
    IConfiguration configuration,
    IMatchesService matchesService,
    IStatisticsService statisticsService,
    IMatchesServiceCached matchesServiceCached,
    IUserAvatar userAvatar) : Controller
{
    private string ServerId => configuration.GetValue<string>("ServerId")!;

    public async Task<IActionResult> Index()
    {
        ViewBag.LastMatches = "active";

        var matches = await matchesServiceCached.GetAsync();

        var communityIds = matches
            .SelectMany(m => m.Teams ?? [])
            .SelectMany(t => t.Players ?? [])
            .Select(p => p.CommunityId);

        await userAvatar.LoadAsync(communityIds);

        return View(matches);
    }

    [Route("details/{start}/{end}")]
    public async Task<IActionResult> Details(string start, string end)
    {
        var matches = await matchesService.BetweenAsync(ServerId, start, end);
        var match = matches.First();
        var statistics = await statisticsService.BetweenAsync(ServerId, start, end);

        var model = new MatchDetailsModel(match, statistics);

        return View(model);
    }
}