using L4D2PlayStats.Core.Matches;
using L4D2PlayStats.Core.Patents.Services;
using L4D2PlayStats.Core.Players.Enums;
using L4D2PlayStats.Core.Players.Extensions;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.Controllers;

public class PlayersController(
    IStringLocalizer<SharedResource> sharedLocalizer,
    IRankingServiceCached rankingService,
    IMatchesServiceCached matchesServiceCached,
    IUserAvatar userAvatar,
    IPatentService patentService) : Controller
{
    [Route("players")]
    public async Task<IActionResult> Index(PlayerResultProperty orderBy = PlayerResultProperty.Experience, bool asc = false, CancellationToken cancellationToken = default)
    {
        ViewBag.Players = "active";

        var playersResult = await rankingService.GetAsync(cancellationToken);
        var communityIds = playersResult.Select(p => p.CommunityId);
        await userAvatar.LoadAsync(communityIds);

        var players = playersResult.SortPlayers(orderBy, asc).ToList();
        var model = new PlayersModel(players, orderBy, asc);

        return View(model);
    }

    [Route("player/{communityId}/{compareWith:long?}")]
    public async Task<IActionResult> Details(long communityId, long? compareWith = null, CancellationToken cancellationToken = default)
    {
        var players = await rankingService.GetAsync(cancellationToken);
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

        var matches = await matchesServiceCached.GetAsync(cancellationToken);

        var communityIds = matches
            .SelectMany(m => m.Teams ?? [])
            .SelectMany(t => t.Players ?? [])
            .Select(p => p.CommunityId);

        await userAvatar.LoadAsync(communityIds);

        var model = new PlayerDetailsModel(firstPlayerRanking, secondPlayerRanking, players, matches);

        return View(model);
    }
}