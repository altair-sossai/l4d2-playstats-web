using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_BunnyHopStreakEvent")]
public class BunnyHopStreakEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.BunnyHopStreak;

    public int Streak { get; set; }
    public double MaxVelocity { get; set; }
}