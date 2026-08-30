using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_TankRockSkeetedEvent")]
public class TankRockSkeetedEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.TankRockSkeeted;

    public Player? Tank { get; set; }
}