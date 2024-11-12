using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Commands;

public class ChatMessageCommand
{
    private string? _message;
    public int When { get; set; }
    public bool Public { get; set; }
    public Team Team { get; set; }
    public string? CommunityId { get; set; }
    public string? Name { get; set; }

    public string? Message
    {
        get => _message;
        set => _message = value?.Trim('"');
    }
}