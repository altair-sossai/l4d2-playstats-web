namespace L4D2PlayStats.Sdk.Matches.Results;

public class MatchResult
{
    public int TeamSize { get; set; }
    public DateTime MatchStart { get; set; }
    public DateTime? MatchEnd { get; set; }
    public TimeSpan? MatchElapsed { get; set; }
    public string? Campaign { get; set; }
    public List<TeamResult>? Teams { get; set; }

    public TeamResult? Winner => Teams?.MaxBy(t => t.Score);
    public TeamResult? TeamA => Teams?.FirstOrDefault();
    public TeamResult? TeamB => Teams?.LastOrDefault();

    public int ScoreA => TeamA?.Score ?? 0;
    public int ScoreB => TeamB?.Score ?? 0;
    public int Diff => Math.Abs(ScoreA - ScoreB);

    public bool Draw => ScoreA == ScoreB;
    public bool TeamAWon => ScoreA > ScoreB;
    public bool TeamBWon => ScoreB > ScoreA;

    public class TeamResult
    {
        public char Code { get; set; }
        public int Score { get; set; }
        public List<PlayerResult>? Players { get; set; }

        /* Survivor */
        public int Died { get; set; }
        public int Incaps { get; set; }
        public int DmgTaken { get; set; }
        public int Common { get; set; }
        public int SiKilled { get; set; }
        public int SiDamage { get; set; }
        public int TankDamage { get; set; }
        public int RockEats { get; set; }
        public int WitchDamage { get; set; }
        public int Skeets { get; set; }
        public int Levels { get; set; }
        public int Crowns { get; set; }
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
    }

    public class PlayerResult
    {
        public string? SteamId { get; set; }
        public string? CommunityId { get; set; }
        public string? Steam3 { get; set; }
        public string? ProfileUrl { get; set; }
        public int Index { get; set; }
        public string? Name { get; set; }

        /* Survivor */
        public int Died { get; set; }
        public decimal DiedPercentage { get; set; }
        public int Incaps { get; set; }
        public decimal IncapsPercentage { get; set; }
        public int DmgTaken { get; set; }
        public decimal DmgTakenPercentage { get; set; }
        public int Common { get; set; }
        public decimal CommonPercentage { get; set; }
        public int SiKilled { get; set; }
        public decimal SiKilledPercentage { get; set; }
        public int SiDamage { get; set; }
        public decimal SiDamagePercentage { get; set; }
        public int TankDamage { get; set; }
        public decimal TankDamagePercentage { get; set; }
        public int RockEats { get; set; }
        public decimal RockEatsPercentage { get; set; }
        public int WitchDamage { get; set; }
        public decimal WitchDamagePercentage { get; set; }
        public int Skeets { get; set; }
        public decimal SkeetsPercentage { get; set; }
        public int Levels { get; set; }
        public decimal LevelsPercentage { get; set; }
        public int Crowns { get; set; }
        public decimal CrownsPercentage { get; set; }
        public int FfGiven { get; set; }
        public decimal FfGivenPercentage { get; set; }

        /* Infected */
        public int DmgTotal { get; set; }
        public decimal DmgTotalPercentage { get; set; }
        public int DmgTank { get; set; }
        public decimal DmgTankPercentage { get; set; }
        public int DmgSpit { get; set; }
        public decimal DmgSpitPercentage { get; set; }
        public int HunterDpDmg { get; set; }
        public decimal HunterDpDmgPercentage { get; set; }

        /* MVP and LVP */
        public int MvpSiDamage { get; set; }
        public decimal MvpSiDamagePercentage { get; set; }
        public int MvpCommon { get; set; }
        public decimal MvpCommonPercentage { get; set; }
        public int TotalMvp => MvpSiDamage + MvpCommon;
        public int LvpFfGiven { get; set; }
        public decimal LvpFfGivenPercentage { get; set; }
    }
}