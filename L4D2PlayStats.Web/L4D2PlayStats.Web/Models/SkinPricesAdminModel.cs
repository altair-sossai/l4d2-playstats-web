using L4D2PlayStats.Core.Skins;
using L4D2PlayStats.Core.Skins.Enums;

namespace L4D2PlayStats.Web.Models;

public class SkinPricesAdminModel(PagedListModel<Skin> skins, IReadOnlyCollection<Skin> allSkins, string? query, SkinRarity? rarity, string? weapon, int customPrices)
{
    public PagedListModel<Skin> Skins { get; } = skins;
    public IReadOnlyCollection<Skin> AllSkins { get; } = allSkins;
    public string? Query { get; } = query;
    public SkinRarity? Rarity { get; } = rarity;
    public string? Weapon { get; } = weapon;
    public int CustomPrices { get; } = customPrices;
}
