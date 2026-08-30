using L4D2PlayStats.Core.GameInfo.Enums;

namespace L4D2PlayStats.Core.GameInfo.Extensions;

public static class GameEventTypeExtensions
{
    public static int Stars(this GameEventType type)
    {
        return type switch
        {
            GameEventType.HunterDeadstop => 1,
            GameEventType.SkeetHurt => 1,
            GameEventType.BoomerPop => 1,
            GameEventType.ChargerLevelHurt => 1,
            GameEventType.TankRockSkeeted => 1,
            GameEventType.SpecialClear => 1,
            GameEventType.SpecialShoved => 1,
            GameEventType.BunnyHopStreak => 1,
            GameEventType.CarAlarmTriggered => 1,
            GameEventType.Skeet => 2,
            GameEventType.WitchCrown => 2,
            GameEventType.WitchCrownHurt => 2,
            GameEventType.SmokerSelfClear => 2,
            GameEventType.HunterHighPounce => 2,
            GameEventType.ChargerLevel => 3,
            GameEventType.TongueCut => 3,
            GameEventType.JockeyHighPounce => 3,
            GameEventType.DeathCharge => 4,
            _ => 0
        };
    }
}