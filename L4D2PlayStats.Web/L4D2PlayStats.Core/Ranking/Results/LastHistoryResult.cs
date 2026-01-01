using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Core.Ranking.Results;

public record LastHistoryResult(HistoryResult? History, List<PlayerResult> Players);
