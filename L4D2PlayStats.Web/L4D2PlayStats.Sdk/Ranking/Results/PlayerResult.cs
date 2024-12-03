namespace L4D2PlayStats.Sdk.Ranking.Results;

public class PlayerResult
{
    private string? _name;

    public long CommunityId { get; set; }
    public string? SteamId { get; set; }
    public string? Steam3 { get; set; }
    public string? ProfileUrl { get; set; }
    public int Position { get; set; }

    public string? Name
    {
        get => string.IsNullOrEmpty(MainName) ? _name : MainName;
        set => _name = value;
    }

    public string? MainName { get; set; }
    public List<PreviousNameResult>? PreviousNames { get; set; }
    public decimal Experience { get; set; }
    public decimal? PreviousExperience { get; set; }
    public decimal? ExperienceDifference { get; set; }
    public int Games { get; set; }
    public int Wins { get; set; }
    public int Loss { get; set; }
    public int RageQuit { get; set; }
    public int Punishment { get; set; }
    public decimal WinRate { get; set; }

    /* Survivor */
    public int Died { get; set; }
    public int Incaps { get; set; }
    public int DmgTaken { get; set; }
    public int Common { get; set; }
    public int SiKilled { get; set; }
    public int SiDamage { get; set; }
    public int TankDamage { get; set; }
    public int RockEats { get; set; }
    public int Skeets { get; set; }
    public int SkeetsMelee { get; set; }
    public int Levels { get; set; }
    public int FfGiven { get; set; }

    /* Infected */
    public int DmgTotal { get; set; }
    public int DmgTank { get; set; }
    public int DmgSpit { get; set; }
    public int HunterDpDmg { get; set; }

    /* MVP and LVP */
    public int MvpSiDamage { get; set; }
    public int MvpCommon { get; set; }
    public int LvpFfGiven { get; set; }

    public List<AnotherPlayerResult>? Anothers { get; set; }
}