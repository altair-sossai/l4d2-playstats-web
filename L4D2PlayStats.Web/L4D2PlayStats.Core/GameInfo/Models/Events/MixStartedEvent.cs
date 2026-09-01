using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_MixStartedEvent")]
public class MixStartedEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.MixStarted;
}