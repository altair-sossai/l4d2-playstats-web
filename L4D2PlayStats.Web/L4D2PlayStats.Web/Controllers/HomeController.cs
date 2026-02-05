using L4D2PlayStats.Core.Patents.Services;
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
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Home = "active";

        var players = await rankingService.GetAsync(cancellationToken);
        var communityIds = players.Select(p => p.CommunityId).ToList();

        await userAvatar.LoadAsync(communityIds);

        var ranking = players.Select(player =>
        {
            var patentProgress = patentService.GetPatentProgress(player);
            var model = new RankingModel(sharedLocalizer, player, patentProgress);

            return model;
        }).ToList();

        if (ranking.Any())
            return View(new HomeModel(ranking));

        var lastHistory = await rankingService.LastHistoryAsync(cancellationToken);
        if (lastHistory == null)
            return View(new HomeModel(ranking));

        communityIds = lastHistory.Players.Select(p => p.CommunityId).ToList();

        await userAvatar.LoadAsync(communityIds);

        return View(new HomeModel(ranking, lastHistory));
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