using L4D2PlayStats.Core.Patents.Services;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using L4D2PlayStats.Sdk.Ranking.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.Controllers;

public class HomeController(
    IStringLocalizer<SharedResource> sharedLocalizer,
    IRankingServiceCached rankingService,
    IUserAvatar userAvatar,
    IPatentService patentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Home = "active";

        var players = await rankingService.GetAsync();
        var communityIds = players.Select(p => p.CommunityId).ToList();

        HistoryResult? lastHistory = null;
        List<PlayerResult>? lastRankingTopFive = null;

        if (players.Count == 0)
        {
            var lastHistoryResult = await rankingService.LastHistoryAsync();

            lastHistory = lastHistoryResult.History;
            lastRankingTopFive = lastHistoryResult.Players;

            communityIds = lastRankingTopFive.Select(p => p.CommunityId).ToList();
        }

        if (communityIds.Count > 0)
            await userAvatar.LoadAsync(communityIds);

        var ranking = players.Select(player =>
        {
            var patentProgress = patentService.GetPatentProgress(player);
            var model = new RankingModel(sharedLocalizer, player, patentProgress);

            return model;
        }).ToList();

        var model = new HomeModel(ranking, lastRankingTopFive, lastHistory);

        return View(model);
    }

    public IActionResult SetTheme(string theme)
    {
        var option = new CookieOptions
        {
            Expires = DateTime.Now.AddYears(1)
        };

        Response.Cookies.Append("theme", theme, option);

        var referer = Request.Headers["Referer"].ToString();

        if (string.IsNullOrEmpty(referer))
            return RedirectToAction("Index");

        return Redirect(referer);
    }
}