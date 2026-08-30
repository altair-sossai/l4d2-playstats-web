using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_JockeyHighPounceEvent")]
public class JockeyHighPounceEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.JockeyHighPounce;

    public Player? Victim { get; set; }
    public double Height { get; set; }
    public bool ReportedHigh { get; set; }
}