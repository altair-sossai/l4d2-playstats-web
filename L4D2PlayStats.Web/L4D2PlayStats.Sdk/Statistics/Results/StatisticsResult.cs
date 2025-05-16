namespace L4D2PlayStats.Sdk.Statistics.Results;

public class StatisticsResult
{
    public string? Server { get; set; }
    public string? FileName { get; set; }
    public string? StatisticId { get; set; }
    public L4D2PlayStats.Statistics? Statistic { get; set; }
}