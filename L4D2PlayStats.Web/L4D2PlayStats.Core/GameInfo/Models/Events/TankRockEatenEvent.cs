using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_TankRockEatenEvent")]
public class TankRockEatenEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.TankRockEaten;

    public Player? Victim { get; set; }
}
