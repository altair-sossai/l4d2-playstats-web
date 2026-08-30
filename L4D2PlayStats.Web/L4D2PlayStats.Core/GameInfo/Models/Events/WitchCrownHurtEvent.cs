using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_WitchCrownHurtEvent")]
public class WitchCrownHurtEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.WitchCrownHurt;

    public int Damage { get; set; }
    public int ChipDamage { get; set; }
}