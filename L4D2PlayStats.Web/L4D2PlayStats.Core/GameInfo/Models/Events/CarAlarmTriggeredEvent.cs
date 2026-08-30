using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_CarAlarmTriggeredEvent")]
public class CarAlarmTriggeredEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.CarAlarmTriggered;

    public Player? Infected { get; set; }
    public CarAlarmReason Reason { get; set; }
}