using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_SkeetEvent")]
public class SkeetEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.Skeet;

    public override int Stars => IsTeamSkeet || Hunter?.IsBot == true ? 1 : base.Stars;

    public Player? Hunter { get; set; }
    public SkeetType SkeetType { get; set; }
    public bool IsTeamSkeet { get; set; }
}