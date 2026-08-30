using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_BoomerVomitLandedEvent")]
public class BoomerVomitLandedEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.BoomerVomitLanded;

    public int Amount { get; set; }
}