using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_BoomerPopEvent")]
public class BoomerPopEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.BoomerPop;

    public Player? Boomer { get; set; }
    public int ShoveCount { get; set; }
    public double TimeAlive { get; set; }
}