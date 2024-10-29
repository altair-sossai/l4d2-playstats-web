using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Ranking.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/ranking")]
[ApiController]
public class RankingController(
    IStringLocalizer<SharedResource> sharedLocalizer,
    IRankingServiceCached rankingService,
    IRankingTemplateService rankingTemplateService,
    IPatentService patentService) : ControllerBase
{
    [HttpGet("plain-text")]
    public async Task<IActionResult> PlainText()
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

        return Ok(new { content });
    }
}