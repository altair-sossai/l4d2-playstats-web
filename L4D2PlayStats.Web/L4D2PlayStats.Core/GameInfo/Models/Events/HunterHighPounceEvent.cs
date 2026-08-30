using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_HunterHighPounceEvent")]
public class HunterHighPounceEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.HunterHighPounce;

    public Player? Victim { get; set; }
    public int Damage { get; set; }
    public double CalculatedDamage { get; set; }
    public double Height { get; set; }
    public bool ReportedHigh { get; set; }
}