namespace L4D2PlayStats.Sdk.Ranking.Results;

public class HistoryResult
{
    public string Id { get; set; } = null!;
    public int StartYear { get; set; }
    public int StartMonth { get; set; }
    public int EndYear { get; set; }
    public int EndMonth { get; set; }
    public bool IsBimonthly { get; set; }
    public bool IsAnnual { get; set; }
}