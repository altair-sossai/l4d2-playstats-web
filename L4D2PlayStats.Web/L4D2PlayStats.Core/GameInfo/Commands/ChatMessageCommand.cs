using L4D2PlayStats.Core.GameInfo.Enums;

namespace L4D2PlayStats.Core.GameInfo.Commands;

public class ChatMessageCommand
{
    public bool Public { get; set; }
    public Team Team { get; set; }
    public string? CommunityId { get; set; }
    public string? Name { get; set; }
    public bool IsAdmin { get; set; }

    public string? Message
    {
        get;
        set => field = value?.Trim();
    }
}