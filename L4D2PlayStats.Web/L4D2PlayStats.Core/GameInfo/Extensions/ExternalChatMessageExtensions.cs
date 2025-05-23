using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.GameInfo.Extensions;

public static class ExternalChatMessageExtensions
{
    public static IEnumerable<ExternalChatMessage> After(this IEnumerable<ExternalChatMessage> messages, long after)
    {
        return messages.Where(x => x.When.Ticks > after);
    }
}