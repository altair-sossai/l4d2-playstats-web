using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class ChatMessage(ChatMessageCommand command)
{
    public long When { get; } = command.When;
    public bool Public { get; } = command.Public;
    public Team Team { get; } = command.Team;

    public Player? Player { get; } = new()
    {
        CommunityId = command.CommunityId,
        Name = command.Name
    };

    public string? Message { get; } = command.Message;
    public bool Empty => string.IsNullOrEmpty(Message);
}