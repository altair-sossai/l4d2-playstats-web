using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_ChargerLevelHurtEvent")]
public class ChargerLevelHurtEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.ChargerLevelHurt;

    public Player? Charger { get; set; }
    public int Damage { get; set; }
}