using System.Globalization;
using L4D2PlayStats.Core.Skins;
using L4D2PlayStats.Core.Skins.Models;

namespace L4D2PlayStats.Web.Models;

public class SkinPurchaseRowModel(SkinPurchaseEntity purchase, Skin? skin, string playerName, string? giftedByName)
{
    public SkinPurchaseEntity Purchase { get; } = purchase;
    public Skin? Skin { get; } = skin;
    public string CommunityId => Purchase.PartitionKey;
    public string PlayerName { get; } = playerName;
    public string? GiftedByName { get; } = giftedByName;

    public string SkinTitle(CultureInfo culture)
    {
        return Skin?.Title.Get(culture) ?? Purchase.SkinId;
    }

    public bool Matches(string? query, CultureInfo culture)
    {
        if (string.IsNullOrWhiteSpace(query))
            return true;

        var term = query.Trim();

        return PlayerName.Contains(term, StringComparison.OrdinalIgnoreCase)
               || CommunityId.Contains(term, StringComparison.OrdinalIgnoreCase)
               || Purchase.SkinId.Contains(term, StringComparison.OrdinalIgnoreCase)
               || SkinTitle(culture).Contains(term, StringComparison.OrdinalIgnoreCase);
    }
}