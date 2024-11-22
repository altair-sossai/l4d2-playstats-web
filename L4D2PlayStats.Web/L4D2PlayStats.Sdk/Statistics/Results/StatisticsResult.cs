namespace L4D2PlayStats.Sdk.Statistics.Results;

public class StatisticsResult
{
    public string? Server { get; set; }
    public string? FileName { get; set; }
    public string? StatisticId { get; set; }
    public StatisticResult? Statistic { get; set; }

    public class StatisticResult
    {
        public GameRoundResult? GameRound { get; set; }
        public List<HalfResult> Halves { get; set; } = [];
        public ScoringResult? Scoring { get; set; }
        public List<PlayerNameResult> PlayerNames { get; set; } = [];
        public List<PlayerNameResult> TeamA { get; set; } = [];
        public List<PlayerNameResult> TeamB { get; set; } = [];
        public DateTime? MapStart { get; set; }
        public DateTime? MapEnd { get; set; }
        public TimeSpan? MapElapsed { get; set; }
    }

    public class GameRoundResult
    {
        public int Round { get; set; }
        public DateTime? When { get; set; }
        public int TeamSize { get; set; }
        public string? ConfigurationName { get; set; }
        public string? MapName { get; set; }
    }

    public class HalfResult
    {
        public static readonly List<HalfResult> EmptyCollection = [];

        public RoundHalfResult? RoundHalf { get; set; }
        public ProgressResult? Progress { get; set; }
        public List<PlayerResult> Players { get; set; } = [];
        public List<InfectedPlayerResult> InfectedPlayers { get; set; } = [];
    }

    public class RoundHalfResult
    {
        public int Round { get; set; }
        public string? Team { get; set; }
        public int Restarts { get; set; }
        public int PillsUsed { get; set; }
        public int KitsUsed { get; set; }
        public int DefibsUsed { get; set; }
        public int Common { get; set; }
        public int SiKilled { get; set; }
        public int SiDamage { get; set; }
        public int SiSpawned { get; set; }
        public int WitchKilled { get; set; }
        public int TankKilled { get; set; }
        public int Incaps { get; set; }
        public int Deaths { get; set; }
        public int FfDamageTotal { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? RoundElapsed { get; set; }
        public DateTime? StartTimePause { get; set; }
        public DateTime? StopTimePause { get; set; }
        public string? PauseElapsed { get; set; }
        public DateTime? StartTimeTank { get; set; }
        public DateTime? StopTimeTank { get; set; }
        public string? TankElapsed { get; set; }
    }

    public class ProgressResult
    {
        public int Round { get; set; }
        public string? Team { get; set; }
        public int Survived { get; set; }
        public int MaxCompletionScore { get; set; }
    }

    public class SteamUserResult
    {
        public string? SteamId { get; set; }
        public string? CommunityId { get; set; }
        public string? Steam3 { get; set; }
        public string? ProfileUrl { get; set; }
    }

    public class PlayerResult : SteamUserResult
    {
        public int Index { get; set; }
        public int Client { get; set; }
        public string? PlayerName { get; set; }
        public int ShotsShotgun { get; set; }
        public int ShotsSmg { get; set; }
        public int ShotsSniper { get; set; }
        public int ShotsPistol { get; set; }
        public int HitsShotgun { get; set; }
        public int HitsSmg { get; set; }
        public int HitsSniper { get; set; }
        public int HitsPistol { get; set; }
        public int HeadshotsSmg { get; set; }
        public int HeadshotsSniper { get; set; }
        public int HeadshotsPistol { get; set; }
        public int HeadshotsSiSmg { get; set; }
        public int HeadshotsSiSniper { get; set; }
        public int HeadshotsSiPistol { get; set; }
        public int HitsSiShotgun { get; set; }
        public int HitsSiSmg { get; set; }
        public int HitsSiSniper { get; set; }
        public int HitsSiPistol { get; set; }
        public int HitsTankShotgun { get; set; }
        public int HitsTankSmg { get; set; }
        public int HitsTankSniper { get; set; }
        public int HitsTankPistol { get; set; }
        public int Common { get; set; }
        public int CommonTankUp { get; set; }
        public int SiKilled { get; set; }
        public int SiKilledTankUp { get; set; }
        public int SiDamage { get; set; }
        public int SiDamageTankUp { get; set; }
        public int Incaps { get; set; }
        public int Died { get; set; }
        public int Skeets { get; set; }
        public int SkeetsHurt { get; set; }
        public int SkeetsMelee { get; set; }
        public int Levels { get; set; }
        public int LevelsHurt { get; set; }
        public int Pops { get; set; }
        public int Crowns { get; set; }
        public int CrownsHurt { get; set; }
        public int Shoves { get; set; }
        public int DeadStops { get; set; }
        public int TongueCuts { get; set; }
        public int SelfClears { get; set; }
        public int FallDamage { get; set; }
        public int DmgTaken { get; set; }
        public int FfGiven { get; set; }
        public int FfTaken { get; set; }
        public int FfHits { get; set; }
        public int TankDamage { get; set; }
        public int WitchDamage { get; set; }
        public int MeleesOnTank { get; set; }
        public int RockSkeets { get; set; }
        public int RockEats { get; set; }
        public int FfGivenPellet { get; set; }
        public int FfGivenBullet { get; set; }
        public int FfGivenSniper { get; set; }
        public int FfGivenMelee { get; set; }
        public int FfGivenFire { get; set; }
        public int FfGivenIncap { get; set; }
        public int FfGivenOther { get; set; }
        public int FfGivenSelf { get; set; }
        public int FfTakenPellet { get; set; }
        public int FfTakenBullet { get; set; }
        public int FfTakenSniper { get; set; }
        public int FfTakenMelee { get; set; }
        public int FfTakenFire { get; set; }
        public int FfTakenIncap { get; set; }
        public int FfTakenOther { get; set; }
        public int FfGivenTotal { get; set; }
        public int FfTakenTotal { get; set; }
        public int Clears { get; set; }
        public int AvgClearTime { get; set; }
        public DateTime? TimeStartPresent { get; set; }
        public DateTime? TimeStopPresent { get; set; }
        public string? TimePresentElapsed { get; set; }
        public DateTime? TimeStartAlive { get; set; }
        public DateTime? TimeStopAlive { get; set; }
        public string? TimeAliveElapsed { get; set; }
        public DateTime? TimeStartUpright { get; set; }
        public DateTime? TimeStopUpright { get; set; }
        public string? TimeUprightElapsed { get; set; }
    }

    public class InfectedPlayerResult : SteamUserResult
    {
        public int Index { get; set; }
        public int Client { get; set; }
        public string? PlayerName { get; set; }
        public int DmgTotal { get; set; }
        public int DmgUpright { get; set; }
        public int DmgTank { get; set; }
        public int DmgTankIncap { get; set; }
        public int DmgScratch { get; set; }
        public int DmgSpit { get; set; }
        public int DmgBoom { get; set; }
        public int DmgTankUp { get; set; }
        public int HunterDPs { get; set; }
        public int HunterDpDmg { get; set; }
        public int JockeyDPs { get; set; }
        public int DeathCharges { get; set; }
        public int Booms { get; set; }
        public int Lerged { get; set; }
        public int Common { get; set; }
        public int Spawns { get; set; }
        public int SpawnSmoker { get; set; }
        public int SpawnBoomer { get; set; }
        public int SpawnHunter { get; set; }
        public int SpawnCharger { get; set; }
        public int SpawnSpitter { get; set; }
        public int SpawnJockey { get; set; }
        public int TankPasses { get; set; }
        public DateTime? TimeStartPresent { get; set; }
        public DateTime? TimeStopPresent { get; set; }
        public string? TimePresentElapsed { get; set; }
    }

    public class ScoringResult
    {
        public TeamResult? TeamA { get; set; }
        public TeamResult? TeamB { get; set; }
        public int? ScoreDiff => Math.Abs((TeamA?.Score ?? 0) - (TeamB?.Score ?? 0));
        public bool Draw => ScoreDiff == 0;
        public bool TeamAWon => TeamA?.Score > TeamB?.Score;
        public bool TeamBWon => TeamB?.Score > TeamA?.Score;
    }

    public class PlayerNameResult : SteamUserResult
    {
        public static readonly List<PlayerNameResult> EmptyCollection = [];

        public int Index { get; set; }
        public string? Name { get; set; }
    }

    public class TeamResult
    {
        public string? Letter { get; set; }
        public int FirstScoresSet { get; set; }
        public int Score { get; set; }
    }
}