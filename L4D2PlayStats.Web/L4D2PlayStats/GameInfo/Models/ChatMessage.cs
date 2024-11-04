using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class ChatMessage
{
    public ChatMessage()
    {
    }

    public ChatMessage(ChatMessageCommand command)
    {
        When = command.When;
        Public = command.Public;
        Team = command.Team;

        Player = new Player
        {
            CommunityId = command.CommunityId,
            Name = command.Name
        };

        Message = command.Message;
    }

    public int When { get; set; }
    public bool Public { get; set; }
    public Team Team { get; set; }
    public Player? Player { get; set; }
    public string? Message { get; set; }
}