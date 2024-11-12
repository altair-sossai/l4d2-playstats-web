using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.Web.Extensions;

public static class MeleeWeaponExtensions
{
    public static bool HasIcon(this MeleeWeapon? meleeWeapon)
    {
        return meleeWeapon is MeleeWeapon.Knife
            or MeleeWeapon.BaseballBat
            or MeleeWeapon.MeleeChainsaw
            or MeleeWeapon.CricketBat
            or MeleeWeapon.Crowbar
            or MeleeWeapon.ElectricGuitar
            or MeleeWeapon.Fireaxe
            or MeleeWeapon.FryingPan
            or MeleeWeapon.GolfClub
            or MeleeWeapon.Katana
            or MeleeWeapon.Machete
            or MeleeWeapon.Tonfa
            or MeleeWeapon.Shovel
            or MeleeWeapon.Pitchfork;
    }
}