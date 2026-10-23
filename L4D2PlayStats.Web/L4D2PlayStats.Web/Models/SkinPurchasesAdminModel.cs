using L4D2PlayStats.Core.Skins;

namespace L4D2PlayStats.Web.Models;

public class SkinPurchasesAdminModel(PagedListModel<SkinPurchaseRowModel> purchases, string? query, string? player, string? playerName, IReadOnlyCollection<Skin> skins, int totalPurchases, int totalStars, int totalGifts)
{
    public PagedListModel<SkinPurchaseRowModel> Purchases { get; } = purchases;
    public string? Query { get; } = query;
    public string? Player { get; } = player;
    public string? PlayerName { get; } = playerName;
    public IReadOnlyCollection<Skin> Skins { get; } = skins;
    public int TotalPurchases { get; } = totalPurchases;
    public int TotalStars { get; } = totalStars;
    public int TotalGifts { get; } = totalGifts;
}