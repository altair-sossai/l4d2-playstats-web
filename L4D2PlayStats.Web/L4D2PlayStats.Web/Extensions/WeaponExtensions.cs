using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.Web.Extensions;

public static class WeaponExtensions
{
    public static bool HasIcon(this Weapon? weapon)
    {
        return weapon is Weapon.Pistol
            or Weapon.Smg
            or Weapon.Pumpshotgun
            or Weapon.Autoshotgun
            or Weapon.Rifle
            or Weapon.HuntingRifle
            or Weapon.SmgSilenced
            or Weapon.ShotgunChrome
            or Weapon.RifleDesert
            or Weapon.SniperMilitary
            or Weapon.ShotgunSpas
            or Weapon.FirstAidKit
            or Weapon.Molotov
            or Weapon.PipeBomb
            or Weapon.PainPills
            or Weapon.Melee
            or Weapon.Chainsaw
            or Weapon.GrenadeLauncher
            or Weapon.Adrenaline
            or Weapon.Defibrillator
            or Weapon.Vomitjar
            or Weapon.RifleAk47
            or Weapon.ColaBottles
            or Weapon.IncendiaryAmmo
            or Weapon.FragAmmo
            or Weapon.PistolMagnum
            or Weapon.SmgMp5
            or Weapon.RifleSg552
            or Weapon.SniperAwp
            or Weapon.SniperScout
            or Weapon.RifleM60;
    }
}