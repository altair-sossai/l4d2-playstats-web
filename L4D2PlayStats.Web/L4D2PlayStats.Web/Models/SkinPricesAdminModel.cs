using L4D2PlayStats.Core.Skins;
using L4D2PlayStats.Core.Skins.Enums;

namespace L4D2PlayStats.Web.Models;

public class SkinPricesAdminModel(PagedListModel<Skin> skins, string? query, SkinRarity? rarity, string? weapon, int customPrices)
{
    public PagedListModel<Skin> Skins { get; } = skins;
    public string? Query { get; } = query;
    public SkinRarity? Rarity { get; } = rarity;
    public string? Weapon { get; } = weapon;
    public int CustomPrices { get; } = customPrices;
}
