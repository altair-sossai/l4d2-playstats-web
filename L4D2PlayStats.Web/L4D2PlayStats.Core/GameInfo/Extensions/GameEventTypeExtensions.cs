using L4D2PlayStats.Core.GameInfo.Enums;

namespace L4D2PlayStats.Core.GameInfo.Extensions;

public static class GameEventTypeExtensions
{
    public static int Stars(this GameEventType type)
    {
        return type switch
        {
            GameEventType.CarAlarmTriggered => 1,
            GameEventType.ChargerLevel => 3,
            GameEventType.ChargerLevelHurt => 1,
            GameEventType.DeathCharge => 4,
            GameEventType.HunterHighPounce => 2,
            GameEventType.Skeet => 2,
            GameEventType.SkeetHurt => 1,
            GameEventType.SmokerSelfClear => 2,
            GameEventType.SpecialClear => 1,
            GameEventType.TankRockSkeeted => 1,
            GameEventType.TongueCut => 3,
            GameEventType.WitchCrown => 2,
            GameEventType.WitchCrownHurt => 2,
            _ => 0
        };
    }
}