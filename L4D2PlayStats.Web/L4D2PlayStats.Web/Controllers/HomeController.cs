using L4D2PlayStats.Core.Patents.Services;
using L4D2PlayStats.Core.Ranking.Results;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
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

        var lastHistoryResult = default(HistoryResult?);

        if (players.Count == 0)
        {
            lastHistoryResult = await rankingService.LastHistoryAsync();

            if (lastHistoryResult != null)
                communityIds = lastHistoryResult.Players.Select(p => p.CommunityId).ToList();
        }

        if (communityIds.Count > 0)
            await userAvatar.LoadAsync(communityIds);

        var ranking = players.Select(player =>
        {
            var patentProgress = patentService.GetPatentProgress(player);
            var model = new RankingModel(sharedLocalizer, player, patentProgress);

            return model;
        }).ToList();

        var model = new HomeModel(ranking, lastHistoryResult);

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
