using System.Text;
using System.Text.Json;
using L4D2PlayStats.OpenAi.Inputs;
using L4D2PlayStats.OpenAi.Results;
using L4D2PlayStats.Sdk.Matches.Results;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace L4D2PlayStats.OpenAi;

public class OpenAiService(IConfiguration configuration, IMemoryCache memoryCache) : IOpenAiService
{
    private static readonly ChatCompletionOptions ChatCompletionOptions = new()
    {
        Temperature = 1
    };

    private string OpenAiApiKey => configuration.GetValue<string>("OpenAiApiKey")!;

    public async Task<ChatResult> LastMatchSummaryAsync(MatchResult match)
    {
        return (await memoryCache.GetOrCreateAsync("OpenAi.LastMatchSummary", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8);

            var players = match.Teams?
                .SelectMany(t => t.Players ?? [])
                .OrderBy(_ => Guid.NewGuid())
                .Take(3)
                .OrderByDescending(o => o.TotalMvp)
                .Select(player => new PlayerInput(player))
                .ToList() ?? [];

            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($$"""
                                       Extraia o desempenho dos jogadores {{string.Join(" e ", players.Select(p => p.Nome))}},
                                       cada um em um parágrafo, de forma resumida, em até 250 caracteres cada parágrafo,
                                       separe cada parágrafo por um |, envolva os nomes dos jogadores em {green}Nome{default}.
                                       """);

            stringBuilder.AppendLine($"JSON: {JsonSerializer.Serialize(players)}");
            stringBuilder.AppendLine("Resultado:");

            var messages = InitialMessages();

            messages.Add(stringBuilder.ToString());

            return await CompleteChatAsync(messages);
        }))!;
    }

    private async Task<ChatResult> CompleteChatAsync(List<ChatMessage> messages)
    {
        var chatClient = new ChatClient("gpt-3.5-turbo", OpenAiApiKey);
        var chatCompletion = await chatClient.CompleteChatAsync(messages, ChatCompletionOptions);
        var chatResult = new ChatResult(chatCompletion);

        return chatResult;
    }

    private static List<ChatMessage> InitialMessages()
    {
        return
        [
            ChatMessage.CreateSystemMessage("Você é um assistente que analisa JSON de partidas de Left 4 Dead 2 para extrair o desempenho dos jogadores")
        ];
    }
}