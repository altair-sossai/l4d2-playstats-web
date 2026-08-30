using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_WitchCrownEvent")]
public class WitchCrownEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.WitchCrown;

    public int Damage { get; set; }
}