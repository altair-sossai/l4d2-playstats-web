using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models.Events;

[FeedPartial("GameEvents/_RoundEndedEvent")]
public class RoundEndedEvent : GameEvent
{
    public override GameEventType EventType => GameEventType.RoundEnded;

    public int SurvivorScore { get; set; }
    public int InfectedScore { get; set; }

    public int Difference => Math.Abs(SurvivorScore - InfectedScore);
}