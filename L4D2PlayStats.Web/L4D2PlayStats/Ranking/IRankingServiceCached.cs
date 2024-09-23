﻿using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Ranking;

public interface IRankingServiceCached
{
    Task<List<PlayerResult>> GetAsync();
    Task<ExperienceConfigResult> ExperienceConfigAsync();
    Task<List<HistoryResult>> AllHistoryAsync();
    Task<List<PlayerResult>> HistoryAsync(string historyId);
}