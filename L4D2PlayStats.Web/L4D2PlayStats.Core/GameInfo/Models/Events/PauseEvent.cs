using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_PauseEvent")]
public class PauseEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.Pause;

    public bool Paused { get; set; }
}
