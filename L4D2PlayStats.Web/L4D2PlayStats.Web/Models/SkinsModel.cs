using L4D2PlayStats.Core.Skins;
using L4D2PlayStats.Core.Skins.Enums;
using L4D2PlayStats.Core.Skins.Results;

namespace L4D2PlayStats.Web.Models;

public class SkinsModel(IReadOnlyCollection<Skin> skins, bool loggedIn, WalletResult? wallet, PlayerSkinsResult? playerSkins)
{
    private const string MeleePrefix = "weapon_melee_";

    public static readonly string[] Weapons =
    [
        "weapon_smg", "weapon_smg_silenced", "weapon_smg_mp5", "weapon_pumpshotgun", "weapon_shotgun_chrome",
        "weapon_melee_fireaxe", "weapon_melee_katana", "weapon_melee_machete", "weapon_melee_baseball_bat", "weapon_melee_cricket_bat",
        "weapon_melee_electric_guitar", "weapon_melee_crowbar", "weapon_melee_golfclub", "weapon_melee_frying_pan"
    ];

    public static readonly IReadOnlyDictionary<string, string> WeaponResourceKeys = new Dictionary<string, string>
    {
        ["weapon_smg"] = "WeaponSmg",
        ["weapon_smg_silenced"] = "WeaponSmgSilenced",
        ["weapon_smg_mp5"] = "WeaponSmgMp5",
        ["weapon_pumpshotgun"] = "WeaponPumpShotgun",
        ["weapon_shotgun_chrome"] = "WeaponShotgunChrome",
        ["weapon_melee_fireaxe"] = "WeaponFireaxe",
        ["weapon_melee_katana"] = "WeaponKatana",
        ["weapon_melee_machete"] = "WeaponMachete",
        ["weapon_melee_baseball_bat"] = "WeaponBaseballBat",
        ["weapon_melee_cricket_bat"] = "WeaponCricketBat",
        ["weapon_melee_electric_guitar"] = "WeaponElectricGuitar",
        ["weapon_melee_crowbar"] = "WeaponCrowbar",
        ["weapon_melee_golfclub"] = "WeaponGolfClub",
        ["weapon_melee_frying_pan"] = "WeaponFryingPan"
    };

    public static readonly SkinRarity[] Rarities = [SkinRarity.Legendary, SkinRarity.Epic, SkinRarity.Rare, SkinRarity.Uncommon, SkinRarity.Common];

    public static bool TestPhase => true;

    public IReadOnlyCollection<Skin> Skins { get; } = skins;
    public bool LoggedIn { get; } = loggedIn;
    public WalletResult? Wallet { get; } = wallet;
    public bool WalletUnavailable => LoggedIn && Wallet == null;
    public PlayerSkinsResult PlayerSkins { get; } = playerSkins ?? new PlayerSkinsResult();

    public IReadOnlyCollection<Skin> OwnedSkins =>
    [
        .. Skins
            .Where(Owns)
            .OrderBy(skin => Array.IndexOf(Weapons, skin.Weapon))
            .ThenByDescending(skin => skin.Rarity)
            .ThenByDescending(skin => skin.Price)
    ];

    public static bool IsMelee(string weapon)
    {
        return weapon.StartsWith(MeleePrefix, StringComparison.Ordinal);
    }

    public static string RarityKey(SkinRarity rarity)
    {
        return $"Rarity{rarity}";
    }

    public static string RarityCss(SkinRarity rarity)
    {
        return $"sk-rarity-{rarity.ToString().ToLowerInvariant()}";
    }

    public IReadOnlyCollection<Skin> SkinsOf(SkinRarity rarity)
    {
        return
        [
            .. Skins
                .Where(skin => skin.Rarity == rarity)
                .OrderByDescending(skin => skin.Price)
                .ThenBy(skin => Array.IndexOf(Weapons, skin.Weapon))
                .ThenBy(skin => skin.Title.En)
        ];
    }

    public bool Owns(Skin skin)
    {
        return PlayerSkins.Owns(skin.Id);
    }

    public bool IsEquipped(Skin skin)
    {
        return PlayerSkins.IsEquipped(skin);
    }

    public bool CanAfford(Skin skin)
    {
        return Wallet != null && Wallet.Balance >= skin.Price;
    }

    public int Missing(Skin skin)
    {
        return Math.Max(0, skin.Price - (Wallet?.Balance ?? 0));
    }
}