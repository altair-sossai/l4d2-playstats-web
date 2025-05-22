using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Enums;

namespace L4D2PlayStats.Core.GameInfo.Models;

public class ChatMessage(ChatMessageCommand command, DateTime? when = null)
{
    public DateTime When { get; } = when ?? DateTime.UtcNow;
    public bool Public { get; } = command.Public;
    public Team Team { get; } = command.Team;

    public Player? Player { get; } = new()
    {
        CommunityId = command.CommunityId,
        Name = command.Name
    };

    public string? Message { get; } = command.Message;
}