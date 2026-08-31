using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_RoundLiveEvent")]
public class RoundLiveEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.RoundLive;

    public bool SecondHalf { get; set; }
}