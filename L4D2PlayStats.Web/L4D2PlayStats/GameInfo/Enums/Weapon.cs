using System.ComponentModel;
using L4D2PlayStats.GameInfo.Attributes;

namespace L4D2PlayStats.GameInfo.Enums;

public enum Weapon
{
    [WeaponName("weapon_none")]
    [Description("None")]
    None,

    [WeaponName("weapon_pistol")]
    [Description("Pistol")]
    Pistol,

    [WeaponName("weapon_smg")]
    [Description("Uzi")]
    Smg,

    [WeaponName("weapon_pumpshotgun")]
    [Description("Pump")]
    Pumpshotgun,

    [WeaponName("weapon_autoshotgun")]
    [Description("Autoshotgun")]
    Autoshotgun,

    [WeaponName("weapon_rifle")]
    [Description("M-16")]
    Rifle,

    [WeaponName("weapon_hunting_rifle")]
    [Description("Hunting Rifle")]
    HuntingRifle,

    [WeaponName("weapon_smg_silenced")]
    [Description("Mac")]
    SmgSilenced,

    [WeaponName("weapon_shotgun_chrome")]
    [Description("Chrome")]
    ShotgunChrome,

    [WeaponName("weapon_rifle_desert")]
    [Description("Desert Rifle")]
    RifleDesert,

    [WeaponName("weapon_sniper_military")]
    [Description("Military Sniper")]
    SniperMilitary,

    [WeaponName("weapon_shotgun_spas")]
    [Description("SPAS Shotgun")]
    ShotgunSpas,

    [WeaponName("weapon_first_aid_kit")]
    [Description("First Aid Kit")]
    FirstAidKit,

    [WeaponName("weapon_molotov")]
    [Description("Molotov")]
    Molotov,

    [WeaponName("weapon_pipe_bomb")]
    [Description("Pipe Bomb")]
    PipeBomb,

    [WeaponName("weapon_pain_pills")]
    [Description("Pills")]
    PainPills,

    [WeaponName("weapon_gascan")]
    [Description("Gascan")]
    Gascan,

    [WeaponName("weapon_propanetank")]
    [Description("Propane Tank")]
    PropaneTank,

    [WeaponName("weapon_oxygentank")]
    [Description("Oxygen Tank")]
    OxygenTank,

    [WeaponName("weapon_melee")]
    [Description("Melee")]
    Melee,

    [WeaponName("weapon_chainsaw")]
    [Description("Chainsaw")]
    Chainsaw,

    [WeaponName("weapon_grenade_launcher")]
    [Description("Grenade Launcher")]
    GrenadeLauncher,

    [WeaponName("weapon_ammo_pack")]
    [Description("Ammo Pack")]
    AmmoPack,

    [WeaponName("weapon_adrenaline")]
    [Description("Adrenaline")]
    Adrenaline,

    [WeaponName("weapon_defibrillator")]
    [Description("Defibrillator")]
    Defibrillator,

    [WeaponName("weapon_vomitjar")]
    [Description("Bile Bomb")]
    Vomitjar,

    [WeaponName("weapon_rifle_ak47")]
    [Description("AK-47")]
    RifleAk47,

    [WeaponName("weapon_gnome")]
    [Description("Gnome")]
    GnomeChompski,

    [WeaponName("weapon_cola_bottles")]
    [Description("Cola Bottles")]
    ColaBottles,

    [WeaponName("weapon_fireworkcrate")]
    [Description("Fireworks")]
    FireworksBox,

    [WeaponName("weapon_upgradepack_incendiary")]
    [Description("Incendiary Ammo Pack")]
    IncendiaryAmmo,

    [WeaponName("weapon_upgradepack_explosive")]
    [Description("Explosive Ammo Pack")]
    FragAmmo,

    [WeaponName("weapon_pistol_magnum")]
    [Description("Deagle")]
    PistolMagnum,

    [WeaponName("weapon_smg_mp5")]
    [Description("MP5")]
    SmgMp5,

    [WeaponName("weapon_rifle_sg552")]
    [Description("SG552")]
    RifleSg552,

    [WeaponName("weapon_sniper_awp")]
    [Description("AWP")]
    SniperAwp,

    [WeaponName("weapon_sniper_scout")]
    [Description("Scout")]
    SniperScout,

    [WeaponName("weapon_rifle_m60")]
    [Description("M60")]
    RifleM60,

    [WeaponName("weapon_tank_claw")]
    [Description("Tank Claw")]
    TankClaw,

    [WeaponName("weapon_hunter_claw")]
    [Description("Hunter Claw")]
    HunterClaw,

    [WeaponName("weapon_charger_claw")]
    [Description("Charger Claw")]
    ChargerClaw,

    [WeaponName("weapon_boomer_claw")]
    [Description("Boomer Claw")]
    BoomerClaw,

    [WeaponName("weapon_smoker_claw")]
    [Description("Smoker Claw")]
    SmokerClaw,

    [WeaponName("weapon_spitter_claw")]
    [Description("Spitter Claw")]
    SpitterClaw,

    [WeaponName("weapon_jockey_claw")]
    [Description("Jockey Claw")]
    JockeyClaw,

    [WeaponName("weapon_machinegun")]
    [Description("Turret")]
    Machinegun,

    [WeaponName("vomit")]
    [Description("vomit")]
    Vomit,

    [WeaponName("splat")]
    [Description("splat")]
    Splat,

    [WeaponName("pounce")]
    [Description("pounce")]
    Pounce,

    [WeaponName("lounge")]
    [Description("lounge")]
    Lounge,

    [WeaponName("pull")]
    [Description("pull")]
    Pull,

    [WeaponName("choke")]
    [Description("choke")]
    Choke,

    [WeaponName("rock")]
    [Description("rock")]
    Rock,

    [WeaponName("physics")]
    [Description("physics")]
    Physics,

    [WeaponName("ammo")]
    [Description("ammo")]
    Ammo,

    [WeaponName("upgrade_item")]
    [Description("upgrade_item")]
    UpgradeItem
}