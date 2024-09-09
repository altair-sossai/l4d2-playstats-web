using L4D2PlayStats.Infrastructure.Attributes;

namespace L4D2PlayStats.Players.Enums;

public enum PlayerResultProperty
{
    [ResourceKey("Experience")]
    Experience,

    [ResourceKey("Games")]
    Games,

    [ResourceKey("Wins")]
    Wins,

    [ResourceKey("Loss")]
    Loss,

    [ResourceKey("RageQuit")]
    RageQuit,

    [ResourceKey("MvpSI")]
    MvpSiDamage,

    [ResourceKey("MvpCI")]
    MvpCommon,

    [ResourceKey("Lvp")]
    LvpFfGiven,

    [ResourceKey("Deaths")]
    Died,

    [ResourceKey("TimesIncap")]
    Incaps,

    [ResourceKey("DamageTaken")]
    DmgTaken,

    [ResourceKey("Commons")]
    Common,

    [ResourceKey("SiKilled")]
    SiKilled,

    [ResourceKey("SiDamage")]
    SiDamage,

    [ResourceKey("TankDamage")]
    TankDamage,

    [ResourceKey("RocksEats")]
    RockEats,

    [ResourceKey("Skeets")]
    Skeets,

    [ResourceKey("Levels")]
    Levels,

    [ResourceKey("FF")]
    FfGiven,

    [ResourceKey("TotalDamage")]
    DmgTotal,

    [ResourceKey("TankDamageDealt")]
    DmgTank,

    [ResourceKey("DamageSpitter")]
    DmgSpit,

    [ResourceKey("HunterPounceDamage")]
    HunterDpDmg
}