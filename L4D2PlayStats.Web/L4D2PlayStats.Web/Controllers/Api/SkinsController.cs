using L4D2PlayStats.Core.Skins.Services;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/skins")]
[ApiController]
public class SkinsController(ISkinService skinService, ISkinShopService skinShopService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var skins = await skinService.GetAllSkinsAsync(cancellationToken);
        var purchases = await skinShopService.GetAllPurchasesAsync(cancellationToken);
        var purchased = purchases.Select(purchase => purchase.SkinId).ToHashSet();

        var result = skins
            .Where(skin => purchased.Contains(skin.Id))
            .Select(skin => new { skin.Id, skin.Weapon })
            .ToList();

        return Ok(result);
    }
}