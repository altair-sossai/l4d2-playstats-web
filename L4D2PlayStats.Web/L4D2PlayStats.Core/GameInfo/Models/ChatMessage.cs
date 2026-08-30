using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Models;

[FeedPartial("_Message")]
public class ChatMessage : FeedItem
{
    public ChatMessage(ChatMessageCommand command, DateTime? when = null)
    {
        When = when ?? DateTime.UtcNow;
        Public = command.Public;
        Team = command.Team;
        Player = new Player
        {
            CommunityId = command.CommunityId,
            Name = command.Name
        };
        IsAdmin = command.IsAdmin;
        Message = command.Message;
    }

    public bool Public { get; }
    public Team Team { get; }
    public Player? Player { get; }
    public bool IsAdmin { get; }
    public string? Message { get; }
}