using OpenAI.Chat;

namespace L4D2PlayStats.OpenAi.Results;

public class ChatResult(ChatCompletion chatCompletion)
{
    public string[]? Phrases { get; set; } = chatCompletion.Content?
        .FirstOrDefault()?.Text?.Split('|', StringSplitOptions.RemoveEmptyEntries)
        .Select(phrase => phrase.Trim())
        .Select(phrase => phrase.StartsWith("{green}") ? phrase : $"{{default}}{phrase}")
        .ToArray();
}