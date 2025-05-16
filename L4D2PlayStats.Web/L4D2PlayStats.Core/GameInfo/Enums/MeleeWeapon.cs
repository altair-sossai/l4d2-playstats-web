using System.ComponentModel;
using L4D2PlayStats.Core.GameInfo.Attributes;

namespace L4D2PlayStats.Core.GameInfo.Enums;

public enum MeleeWeapon
{
    [WeaponName("")]
    [Description("None")]
    MeleeNone,

    [WeaponName("knife")]
    [Description("Knife")]
    Knife,

    [WeaponName("baseball_bat")]
    [Description("Baseball Bat")]
    BaseballBat,

    [WeaponName("chainsaw")]
    [Description("Chainsaw")]
    MeleeChainsaw,

    [WeaponName("cricket_bat")]
    [Description("Cricket Bat")]
    CricketBat,

    [WeaponName("crowbar")]
    [Description("Crowbar")]
    Crowbar,

    [WeaponName("didgeridoo")]
    [Description("didgeridoo")]
    Didgeridoo,

    [WeaponName("electric_guitar")]
    [Description("Guitar")]
    ElectricGuitar,

    [WeaponName("fireaxe")]
    [Description("Axe")]
    Fireaxe,

    [WeaponName("frying_pan")]
    [Description("Frying Pan")]
    FryingPan,

    [WeaponName("golfclub")]
    [Description("Golf Club")]
    GolfClub,

    [WeaponName("katana")]
    [Description("Katana")]
    Katana,

    [WeaponName("machete")]
    [Description("Machete")]
    Machete,

    [WeaponName("riotshield")]
    [Description("Riot Shield")]
    RiotShield,

    [WeaponName("tonfa")]
    [Description("Tonfa")]
    Tonfa,

    [WeaponName("shovel")]
    [Description("Shovel")]
    Shovel,

    [WeaponName("pitchfork")]
    [Description("Pitchfork")]
    Pitchfork
}