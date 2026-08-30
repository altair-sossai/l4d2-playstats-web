using L4D2PlayStats.Core.GameInfo.Models.Events;

namespace L4D2PlayStats.Core.GameInfo.Extensions;

public static class GameEventExtensions
{
    public static IEnumerable<GameEvent> After(this IEnumerable<GameEvent> events, long after)
    {
        return events.Where(x => x.When.Ticks > after);
    }
}