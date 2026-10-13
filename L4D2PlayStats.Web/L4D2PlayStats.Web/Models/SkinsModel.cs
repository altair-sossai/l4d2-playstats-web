using L4D2PlayStats.Core.Patents;
using L4D2PlayStats.Core.Skins;

namespace L4D2PlayStats.Web.Models;

public class SkinsModel(IReadOnlyCollection<Patent> patents, IReadOnlyCollection<Skin> skins, Patent? playerPatent)
{
    public static readonly string[] Weapons = ["weapon_smg", "weapon_smg_silenced", "weapon_pumpshotgun", "weapon_shotgun_chrome"];

    public IReadOnlyCollection<Patent> Patents { get; } = [.. patents.OrderByDescending(patent => patent.Level)];
    public IReadOnlyCollection<Skin> Skins { get; } = skins;
    public Patent? PlayerPatent { get; } = playerPatent;
    public int UnlockedCount => PlayerPatent == null ? 0 : Skins.Count(CanUse);

    public bool CanUse(Skin skin)
    {
        return PlayerPatent != null && skin.Level <= PlayerPatent.Level;
    }

    public IReadOnlyCollection<Skin> SkinsOf(int level)
    {
        return Skins
            .Where(skin => skin.Level == level)
            .OrderBy(skin => Array.IndexOf(Weapons, skin.Weapon))
            .ThenBy(skin => skin.Title.En)
            .ToList();
    }
}