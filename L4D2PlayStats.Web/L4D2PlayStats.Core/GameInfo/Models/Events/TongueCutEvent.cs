using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_TongueCutEvent")]
public class TongueCutEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.TongueCut;

    public Player? Smoker { get; set; }
}