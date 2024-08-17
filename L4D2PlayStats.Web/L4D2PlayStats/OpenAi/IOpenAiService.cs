using L4D2PlayStats.OpenAi.Results;
using L4D2PlayStats.Sdk.Matches.Results;

namespace L4D2PlayStats.OpenAi;

public interface IOpenAiService
{
    Task<ChatResult> LastMatchSummaryAsync(MatchResult match);
}