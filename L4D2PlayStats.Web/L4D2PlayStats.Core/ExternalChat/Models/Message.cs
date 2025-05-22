using L4D2PlayStats.Core.ExternalChat.Commands;
using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.GameInfo.Models;

namespace L4D2PlayStats.Core.ExternalChat.Models;

public class Message(User user, MessageCommand command)
{
    public DateTime When { get; } = DateTime.UtcNow;
    public TimeSpan Age => DateTime.UtcNow - When;
    public string? SteamId { get; } = user.SteamId;
    public string? CommunityId { get; } = user.CommunityId;
    public string? Name { get; } = user.Name;
    public string? Text { get; } = command.Message;

    public static implicit operator ChatMessage(Message message)
    {
        return new ChatMessage(new ChatMessageCommand
        {
            Public = false,
            Team = Team.External,
            CommunityId = message.CommunityId,
            Name = message.Name,
            Message = message.Text
        }, message.When);
    }
}