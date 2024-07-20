namespace L4D2PlayStats.Sdk.Ranking.Results;

public class PlayerResult
{
    public long CommunityId { get; set; }
    public string? SteamId { get; set; }
    public string? Steam3 { get; set; }
    public string? ProfileUrl { get; set; }
    public int Position { get; set; }
    public string? Name { get; set; }
    public decimal Experience { get; set; }
    public decimal? PreviousExperience { get; set; }
    public int Games { get; set; }
    public int Wins { get; set; }
    public int Loss { get; set; }
    public decimal WinRate { get; set; }
    public int Mvps { get; set; }
    public int MvpsCommon { get; set; }
}