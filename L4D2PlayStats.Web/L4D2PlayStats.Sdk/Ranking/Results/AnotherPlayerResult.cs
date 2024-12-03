namespace L4D2PlayStats.Sdk.Ranking.Results;

public class AnotherPlayerResult
{
    public static readonly List<AnotherPlayerResult> EmptyCollection = [];

    public long? CommunityId { get; set; }
    public string? Name { get; set; }
    public int WinsWith { get; set; }
    public int LossWith { get; set; }
    public int GamesWith => WinsWith + LossWith;
    public decimal WinRateWith => GamesWith == 0 ? 0 : WinsWith / (decimal)GamesWith;
    public decimal LossRateWith => GamesWith == 0 ? 0 : LossWith / (decimal)GamesWith;
    public int WinsAgainst { get; set; }
    public int LossAgainst { get; set; }
    public int GamesAgainst => WinsAgainst + LossAgainst;
    public decimal WinRateAgainst => GamesAgainst == 0 ? 0 : WinsAgainst / (decimal)GamesAgainst;
    public decimal LossRateAgainst => GamesAgainst == 0 ? 0 : LossAgainst / (decimal)GamesAgainst;
}