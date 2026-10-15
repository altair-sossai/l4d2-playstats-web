using L4D2PlayStats.Core.Skins;
using L4D2PlayStats.Core.Skins.Results;

namespace L4D2PlayStats.Web.Models;

public class PlayerSkinsModel(IReadOnlyCollection<Skin> skins, PlayerSkinsResult playerSkins, WalletResult? wallet)
{
    public IReadOnlyCollection<Skin> OwnedSkins { get; } =
    [
        .. skins
            .Where(skin => playerSkins.Owns(skin.Id))
            .OrderByDescending(skin => skin.Rarity)
            .ThenByDescending(skin => skin.Price)
    ];

    public PlayerSkinsResult PlayerSkins { get; } = playerSkins;
    public WalletResult? Wallet { get; } = wallet;

    public bool IsEquipped(Skin skin)
    {
        return PlayerSkins.IsEquipped(skin);
    }
}