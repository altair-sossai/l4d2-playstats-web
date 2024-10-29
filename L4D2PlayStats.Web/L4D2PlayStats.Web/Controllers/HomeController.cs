using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Ranking.Services;
using L4D2PlayStats.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.Controllers;

public class HomeController(
    IStringLocalizer<SharedResource> sharedLocalizer,
    IRankingServiceCached rankingService,
    IRankingTemplateService rankingTemplateService,
    IUserAvatar userAvatar,
    IPatentService patentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Home = "active";

        var players = await rankingService.GetAsync();
        var communityIds = players.Select(p => p.CommunityId);
        await userAvatar.LoadAsync(communityIds);

        var ranking = players.Select(player =>
        {
            var patentProgress = patentService.GetPatentProgress(player);
            var model = new RankingModel(sharedLocalizer, player, patentProgress);

            return model;
        }).ToList();

        var model = new HomeModel(ranking);

        return View(model);
    }

    [Route("ranking/plain-text")]
    public async Task<ActionResult> PlainText()
    {
        var players = await rankingService.GetAsync();
        var ranking = players.Select(player =>
        {
            var patentProgress = patentService.GetPatentProgress(player);
            var model = new RankingModel(sharedLocalizer, player, patentProgress);

            return model;
        }).ToList();

        var model = new HomeModel(ranking);
        var content = rankingTemplateService.Render(model);

        return Content(content);
    }

    public IActionResult SetTheme(string theme)
    {
        var option = new CookieOptions
        {
            Expires = DateTime.Now.AddYears(1)
        };

        Response.Cookies.Append("theme", theme, option);

        return Redirect(Request.Headers["Referer"].ToString());
    }
}