using L4D2PlayStats.Matches;
using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Ranking;
using L4D2PlayStats.UserAvatar;
using L4D2PlayStats.Web.Models;
using L4D2PlayStats.Web.Models.OrderBy.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.Controllers;

public class PlayerController(
    IStringLocalizer<SharedResource> sharedLocalizer,
    IRankingServiceCached rankingService,
    IMatchesServiceCached matchesServiceCached,
    IUserAvatar userAvatar,
    IPatentService patentService) : Controller
{
    [Route("player/{communityId}/{compareWith:long?}")]
    public async Task<IActionResult> Index(long communityId, long? compareWith = null)
    {
        var players = await rankingService.GetAsync();
        var firstPlayer = players.FirstOrDefault(p => p.CommunityId == communityId);
        var secondPlayer = compareWith == null ? null : players.FirstOrDefault(p => p.CommunityId == compareWith);

        if (firstPlayer == null)
            return View("PlayerNotFound");

        await userAvatar.LoadAsync(firstPlayer.CommunityId);

        if (secondPlayer != null)
            await userAvatar.LoadAsync(secondPlayer.CommunityId);

        var firstPlayerPatentProgress = patentService.GetPatentProgress(firstPlayer);
        var secondPlayerPatentProgress = secondPlayer == null ? null : patentService.GetPatentProgress(secondPlayer);

        var firstPlayerRanking = new RankingModel(sharedLocalizer, firstPlayer, firstPlayerPatentProgress);
        var secondPlayerRanking = secondPlayer == null || secondPlayerPatentProgress == null ? null : new RankingModel(sharedLocalizer, secondPlayer, secondPlayerPatentProgress);

        var matches = await matchesServiceCached.GetAsync();

        var communityIds = matches
            .SelectMany(m => m.Teams ?? [])
            .SelectMany(t => t.Players ?? [])
            .Select(p => p.CommunityId);

        await userAvatar.LoadAsync(communityIds);

        var model = new PlayerDetailsModel(firstPlayerRanking, secondPlayerRanking, players, matches);

        return View(model);
    }

    [Route("players")]
    public IActionResult All(OrderByFilter orderBy)
    {        
        return RedirectToAction("Index", "Home", new { orderBy });
    }
}