using L4D2PlayStats.Core.Patents;
using L4D2PlayStats.Core.Skins;

namespace L4D2PlayStats.Web.Models;

public class SkinsModel(IReadOnlyCollection<Patent> patents, IReadOnlyCollection<Skin> skins, Patent? playerPatent)
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

    public IReadOnlyCollection<Patent> Patents { get; } = [.. patents.OrderByDescending(patent => patent.Level)];
    public IReadOnlyCollection<Skin> Skins { get; } = skins;
    public Patent? PlayerPatent { get; } = playerPatent;
    public IReadOnlyCollection<Skin> Released { get; } = [.. skins.Where(skin => skin.IsReleased)];
    public int UnlockedCount => PlayerPatent == null ? 0 : Released.Count(CanUse);

    public IReadOnlyCollection<IGrouping<DateOnly, Skin>> UpcomingLots { get; } =
    [
        .. skins
            .Where(skin => !skin.IsReleased)
            .OrderBy(skin => skin.ReleaseDate)
            .ThenByDescending(skin => skin.Level)
            .ThenBy(skin => Array.IndexOf(Weapons, skin.Weapon))
            .GroupBy(skin => skin.ReleaseDate)
    ];

    public static bool IsMelee(string weapon)
    {
        return weapon.StartsWith(MeleePrefix, StringComparison.Ordinal);
    }

    public bool CanUse(Skin skin)
    {
        return skin.IsReleased && PlayerPatent != null && skin.Level <= PlayerPatent.Level;
    }

    public Patent PatentOf(Skin skin)
    {
        return Patents.First(patent => patent.Level == skin.Level);
    }

    public IReadOnlyCollection<Skin> SkinsOf(int level)
    {
        return Released
            .Where(skin => skin.Level == level)
            .OrderBy(skin => Array.IndexOf(Weapons, skin.Weapon))
            .ThenBy(skin => skin.Title.En)
            .ToList();
    }
}
