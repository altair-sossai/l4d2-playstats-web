using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Ranking;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/players")]
[ApiController]
public class PlayersController(IRankingServiceCached rankingService, IPatentService patentService) : ControllerBase
{
    [HttpGet("patents")]
    public async Task<IActionResult> Patents()
    {
        var players = await rankingService.GetAsync();

        var models = players
            .Take(50)
            .Select(player =>
            {
                var patentProgress = patentService.GetPatentProgress(player);
                var model = new PlayerPatentModel(player, patentProgress);

                return model;
            }).ToList();

        return Ok(models);
    }
}