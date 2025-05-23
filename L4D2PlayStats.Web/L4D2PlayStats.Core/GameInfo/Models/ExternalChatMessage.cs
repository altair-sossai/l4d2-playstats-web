using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Enums;

namespace L4D2PlayStats.Core.GameInfo.Models;

public class ExternalChatMessage(User user, ExternalChatMessageCommand command)
{
    public DateTime When { get; } = DateTime.UtcNow;
    public TimeSpan Age => DateTime.UtcNow - When;
    public string Ticks => When.Ticks.ToString();
    public string? SteamId { get; } = user.SteamId;
    public string? CommunityId { get; } = user.CommunityId;
    public string? Name { get; } = user.Name;
    public string? Text { get; } = command.Message;

    public static implicit operator ChatMessage(ExternalChatMessage externalChatMessage)
    {
        return new ChatMessage(new ChatMessageCommand
        {
            Public = false,
            Team = Team.External,
            CommunityId = externalChatMessage.CommunityId,
            Name = externalChatMessage.Name,
            Message = externalChatMessage.Text
        }, externalChatMessage.When);
    }
}