using OpenAI.Chat;

namespace L4D2PlayStats.OpenAi.Results;

public class ChatResult(ChatCompletion chatCompletion)
{
    public string[]? Phrases { get; set; } = chatCompletion.Content?
        .FirstOrDefault()?.Text?.Split('|', StringSplitOptions.RemoveEmptyEntries)
        .Select(v => v.Trim())
        .ToArray();
}