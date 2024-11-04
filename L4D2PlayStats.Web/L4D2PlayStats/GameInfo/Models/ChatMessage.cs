using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class ChatMessage
{
    public int When { get; set; }
    public bool Public { get; set; }
    public Team Team { get; set; }
    public Player? Player { get; set; }
    public string? Message { get; set; }
}