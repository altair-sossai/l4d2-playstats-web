using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_DeathChargeEvent")]
public class DeathChargeEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.DeathCharge;

    public Player? Victim { get; set; }
    public double Height { get; set; }
    public double Distance { get; set; }
    public bool WasCarried { get; set; }
}