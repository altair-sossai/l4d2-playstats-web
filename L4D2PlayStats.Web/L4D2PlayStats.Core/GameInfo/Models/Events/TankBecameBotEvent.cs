using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_TankBecameBotEvent")]
public class TankBecameBotEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.TankBecameBot;
}
