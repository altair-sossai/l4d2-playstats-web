using L4D2PlayStats.Core.Patents.Services;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.Skins.Commands;
using L4D2PlayStats.Core.Skins.Services;
using L4D2PlayStats.Web.Attributes;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/players")]
[ApiController]
public class PlayersController(IRankingServiceCached rankingService, IPatentService patentService, ISkinShopService skinShopService) : ControllerBase
{
    [HttpGet("patents")]
    public async Task<IActionResult> Patents(CancellationToken cancellationToken)
    {
        var players = await rankingService.GetAsync(cancellationToken);

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

    [HttpGet("skins")]
    public async Task<IActionResult> SkinsAsync(CancellationToken cancellationToken)
    {
        var players = await skinShopService.GetAllPlayerSkinsAsync(cancellationToken);

        var result = players.Select(player => new
        {
            player.CommunityId,
            skins = player.Owned.Order(),
            equipped = player.Equipped.Select(pair => new { weapon = pair.Key, skin = pair.Value })
        });

        return Ok(result);
    }

    [HttpGet("{communityId:long}/wallet")]
    public async Task<IActionResult> WalletAsync(long communityId, CancellationToken cancellationToken)
    {
        var wallet = await skinShopService.GetWalletAsync(communityId.ToString(), cancellationToken);
        if (wallet == null)
            return StatusCode(StatusCodes.Status503ServiceUnavailable);

        return Ok(wallet);
    }

    [HttpPost("{communityId:long}/skins/equip")]
    [RequiredSecretKey]
    public async Task<IActionResult> EquipAsync(long communityId, [FromBody] EquipSkinCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(command.Weapon))
            return BadRequest();

        var result = string.IsNullOrEmpty(command.Skin)
            ? await skinShopService.UnequipAsync(communityId.ToString(), command.Weapon, "game", cancellationToken)
            : await skinShopService.EquipAsync(communityId.ToString(), command.Skin, "game", cancellationToken);

        return Ok(new { result.Status, result.Succeeded });
    }
}