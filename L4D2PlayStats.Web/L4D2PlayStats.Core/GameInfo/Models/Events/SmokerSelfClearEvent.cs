using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_SmokerSelfClearEvent")]
public class SmokerSelfClearEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.SmokerSelfClear;

    public Player? Smoker { get; set; }
    public bool WithShove { get; set; }
}