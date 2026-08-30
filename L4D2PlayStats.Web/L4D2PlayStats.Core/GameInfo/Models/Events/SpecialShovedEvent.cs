using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_SpecialShovedEvent")]
public class SpecialShovedEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.SpecialShoved;

    public Player? Infected { get; set; }
    public InfectedType ZombieClass { get; set; }
}