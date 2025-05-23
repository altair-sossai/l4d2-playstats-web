using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.GameInfo.Extensions;

public static class ChatMessageExtensions
{
    public static IEnumerable<ChatMessage> After(this IEnumerable<ChatMessage> messages, long after)
    {
        return messages.Where(x => x.When.Ticks > after);
    }
}