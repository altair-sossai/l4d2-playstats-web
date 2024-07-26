using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Ranking;
using L4D2PlayStats.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class PlayerController(IRankingServiceCached rankingService, IUserAvatar userAvatar, IPatentService patentService) : Controller
{
    [Route("player/{communityId}")]
    public async Task<IActionResult> Index(long communityId)
    {
        var players = await rankingService.GetAsync();
        var player = players.FirstOrDefault(p => p.CommunityId == communityId);

        if (player == null)
            return NotFound();

        await userAvatar.LoadAsync(player.CommunityId);

        var patentProgress = patentService.GetPatentProgress(player);
        var model = new RankingModel(player, patentProgress);

        return View(model);
    }
}