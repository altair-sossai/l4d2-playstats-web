using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.Skins.Results;
using L4D2PlayStats.Core.Skins.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class SkinsController(ISkinService skinService, ISkinShopService skinShopService, ICurrentUser currentUser) : Controller
{
    [Route("skins")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Skins = "active";

        var skins = await skinService.GetAllSkinsAsync(cancellationToken);

        WalletResult? wallet = null;
        PlayerSkinsResult? playerSkins = null;

        if (!string.IsNullOrEmpty(currentUser.CommunityId))
        {
            wallet = await skinShopService.GetWalletAsync(currentUser.CommunityId, cancellationToken);
            playerSkins = await skinShopService.GetPlayerSkinsAsync(currentUser.CommunityId, cancellationToken);
        }

        var model = new SkinsModel(skins, currentUser.IsAuthenticated, wallet, playerSkins);

        return View(model);
    }
}