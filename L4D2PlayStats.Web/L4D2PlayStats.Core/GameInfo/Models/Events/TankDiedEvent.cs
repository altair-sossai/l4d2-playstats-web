using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_TankDiedEvent")]
public class TankDiedEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.TankDied;
}