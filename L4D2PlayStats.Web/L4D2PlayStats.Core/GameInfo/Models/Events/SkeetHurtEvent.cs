using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_SkeetHurtEvent")]
public class SkeetHurtEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.SkeetHurt;

    public Player? Hunter { get; set; }
    public SkeetType SkeetType { get; set; }
    public int Damage { get; set; }
    public bool IsOverkill { get; set; }
}