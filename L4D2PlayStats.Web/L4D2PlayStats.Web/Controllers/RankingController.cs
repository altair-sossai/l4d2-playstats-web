using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Ranking;
using L4D2PlayStats.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.Controllers;

public class RankingController(
    IStringLocalizer<SharedResource> sharedLocalizer,
    IRankingServiceCached rankingService,
    IUserAvatar userAvatar,
    IPatentService patentService) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Ranking = "active";

        var players = await rankingService.GetAsync();
        var communityIds = players.Select(p => p.CommunityId);
        await userAvatar.LoadAsync(communityIds);

        var models = players.Select(player =>
        {
            var patentProgress = patentService.GetPatentProgress(player);
            var model = new RankingModel(sharedLocalizer, player, patentProgress);

            return model;
        }).ToList();

        return View(models);
    }
}