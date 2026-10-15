using System.Globalization;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.Skins;
using L4D2PlayStats.Core.Skins.Enums;
using L4D2PlayStats.Core.Skins.Services;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Attributes;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

[Authorize]
[AdminAuthorize]
public class SkinsAdminController(
    ISkinService skinService,
    ISkinShopService skinShopService,
    IRankingServiceCached rankingService,
    IUserAvatar userAvatar) : Controller
{
    private const int PageSize = 20;

    [Route("admin/skins/purchases")]
    public async Task<IActionResult> Purchases(string? q, int page = 1, CancellationToken cancellationToken = default)
    {
        ViewBag.Admin = "active";
        ViewBag.SkinPurchases = "active";

        var culture = CultureInfo.CurrentUICulture;
        var skins = await skinService.GetAllSkinsAsync(cancellationToken);
        var skinsById = skins.ToDictionary(skin => skin.Id);
        var purchases = await skinShopService.GetAllPurchasesAsync(cancellationToken);
        var players = await rankingService.GetAsync(cancellationToken);

        var names = players
            .Where(player => !string.IsNullOrEmpty(player.Name))
            .GroupBy(player => player.CommunityId.ToString(CultureInfo.InvariantCulture))
            .ToDictionary(group => group.Key, group => group.First().Name!);

        var rows = purchases
            .OrderByDescending(purchase => purchase.PurchasedAtUtc)
            .Select(purchase => new SkinPurchaseRowModel(
                purchase,
                skinsById.GetValueOrDefault(purchase.SkinId),
                purchase.PlayerName ?? names.GetValueOrDefault(purchase.PartitionKey) ?? purchase.PartitionKey,
                purchase.GiftedByCommunityId == null ? null : purchase.GiftedByName ?? names.GetValueOrDefault(purchase.GiftedByCommunityId) ?? purchase.GiftedByCommunityId))
            .Where(row => row.Matches(q, culture))
            .ToList();

        var list = new PagedListModel<SkinPurchaseRowModel>(rows, page, PageSize);

        await userAvatar.LoadAsync(list.Items.Select(row => row.CommunityId), false, cancellationToken);

        var model = new SkinPurchasesAdminModel(list, q, skins, purchases.Count(purchase => !purchase.Gift), purchases.Sum(purchase => purchase.Price), purchases.Count(purchase => purchase.Gift));

        return View(model);
    }

    [Route("admin/skins/prices")]
    public async Task<IActionResult> Prices(string? q, SkinRarity? rarity, string? weapon, int page = 1, CancellationToken cancellationToken = default)
    {
        ViewBag.Admin = "active";
        ViewBag.SkinPrices = "active";

        var culture = CultureInfo.CurrentUICulture;
        var skins = await skinService.GetAllSkinsAsync(cancellationToken);
        var term = q?.Trim();

        var filtered = skins
            .Where(skin => rarity == null || skin.Rarity == rarity)
            .Where(skin => string.IsNullOrEmpty(weapon) || skin.Weapon == weapon)
            .Where(skin => string.IsNullOrEmpty(term)
                           || skin.Id.Contains(term, StringComparison.OrdinalIgnoreCase)
                           || skin.Title.Get(culture).Contains(term, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(skin => skin.Rarity)
            .ThenByDescending(skin => skin.Price)
            .ThenBy(skin => Array.IndexOf(SkinsModel.Weapons, skin.Weapon))
            .ToList();

        var list = new PagedListModel<Skin>(filtered, page, PageSize);
        var model = new SkinPricesAdminModel(list, skins, q, rarity, weapon, skins.Count(skin => skin.CustomPrice != null));

        return View(model);
    }
}
