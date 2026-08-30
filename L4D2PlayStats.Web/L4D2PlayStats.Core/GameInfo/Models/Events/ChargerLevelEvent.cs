using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_ChargerLevelEvent")]
public class ChargerLevelEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.ChargerLevel;

    public Player? Charger { get; set; }
}