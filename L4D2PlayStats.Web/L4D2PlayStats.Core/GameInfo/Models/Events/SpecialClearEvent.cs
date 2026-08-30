using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_SpecialClearEvent")]
public class SpecialClearEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.SpecialClear;

    public Player? Pinner { get; set; }
    public Player? PinVictim { get; set; }
    public InfectedType ZombieClass { get; set; }
    public double TimeA { get; set; }
    public double TimeB { get; set; }
    public bool WithShove { get; set; }
}