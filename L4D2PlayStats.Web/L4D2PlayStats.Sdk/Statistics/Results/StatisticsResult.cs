using L4D2PlayStats.Sdk.Statistics.Enums;
using L4D2PlayStats.Sdk.Statistics.Extensions;

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

        public long this[RoundStats roundStats]
        {
            get
            {
                return roundStats switch
                {
                    RoundStats.Restarts => Restarts,
                    RoundStats.PillsUsed => PillsUsed,
                    RoundStats.KitsUsed => KitsUsed,
                    RoundStats.DefibsUsed => DefibsUsed,
                    RoundStats.Common => Common,
                    RoundStats.SiKilled => SiKilled,
                    RoundStats.SiDamage => SiDamage,
                    RoundStats.SiSpawned => SiSpawned,
                    RoundStats.WitchKilled => WitchKilled,
                    RoundStats.TankKilled => TankKilled,
                    RoundStats.Incaps => Incaps,
                    RoundStats.Deaths => Deaths,
                    RoundStats.FfDamageTotal => FfDamageTotal,
                    RoundStats.StartTime => StartTime.ToUnixTimeSeconds(),
                    RoundStats.EndTime => EndTime.ToUnixTimeSeconds(),
                    RoundStats.StartTimePause => StartTimePause.ToUnixTimeSeconds(),
                    RoundStats.StopTimePause => StopTimePause.ToUnixTimeSeconds(),
                    RoundStats.StartTimeTank => StartTimeTank.ToUnixTimeSeconds(),
                    RoundStats.StopTimeTank => StopTimeTank.ToUnixTimeSeconds(),
                    _ => throw new ArgumentOutOfRangeException(nameof(roundStats), roundStats, null)
                };
            }
        }
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

        public long this[PlayerStats playerStats]
        {
            get
            {
                return playerStats switch
                {
                    PlayerStats.ShotsShotgun => ShotsShotgun,
                    PlayerStats.ShotsSmg => ShotsSmg,
                    PlayerStats.ShotsSniper => ShotsSniper,
                    PlayerStats.ShotsPistol => ShotsPistol,
                    PlayerStats.HitsShotgun => HitsShotgun,
                    PlayerStats.HitsSmg => HitsSmg,
                    PlayerStats.HitsSniper => HitsSniper,
                    PlayerStats.HitsPistol => HitsPistol,
                    PlayerStats.HeadshotsSmg => HeadshotsSmg,
                    PlayerStats.HeadshotsSniper => HeadshotsSniper,
                    PlayerStats.HeadshotsPistol => HeadshotsPistol,
                    PlayerStats.HeadshotsSiSmg => HeadshotsSiSmg,
                    PlayerStats.HeadshotsSiSniper => HeadshotsSiSniper,
                    PlayerStats.HeadshotsSiPistol => HeadshotsSiPistol,
                    PlayerStats.HitsSiShotgun => HitsSiShotgun,
                    PlayerStats.HitsSiSmg => HitsSiSmg,
                    PlayerStats.HitsSiSniper => HitsSiSniper,
                    PlayerStats.HitsSiPistol => HitsSiPistol,
                    PlayerStats.HitsTankShotgun => HitsTankShotgun,
                    PlayerStats.HitsTankSmg => HitsTankSmg,
                    PlayerStats.HitsTankSniper => HitsTankSniper,
                    PlayerStats.HitsTankPistol => HitsTankPistol,
                    PlayerStats.Common => Common,
                    PlayerStats.CommonTankUp => CommonTankUp,
                    PlayerStats.SiKilled => SiKilled,
                    PlayerStats.SiKilledTankUp => SiKilledTankUp,
                    PlayerStats.SiDamage => SiDamage,
                    PlayerStats.SiDamageTankUp => SiDamageTankUp,
                    PlayerStats.Incaps => Incaps,
                    PlayerStats.Died => Died,
                    PlayerStats.Skeets => Skeets,
                    PlayerStats.SkeetsHurt => SkeetsHurt,
                    PlayerStats.SkeetsMelee => SkeetsMelee,
                    PlayerStats.Levels => Levels,
                    PlayerStats.LevelsHurt => LevelsHurt,
                    PlayerStats.Pops => Pops,
                    PlayerStats.Crowns => Crowns,
                    PlayerStats.CrownsHurt => CrownsHurt,
                    PlayerStats.Shoves => Shoves,
                    PlayerStats.DeadStops => DeadStops,
                    PlayerStats.TongueCuts => TongueCuts,
                    PlayerStats.SelfClears => SelfClears,
                    PlayerStats.FallDamage => FallDamage,
                    PlayerStats.DmgTaken => DmgTaken,
                    PlayerStats.FfGiven => FfGiven,
                    PlayerStats.FfTaken => FfTaken,
                    PlayerStats.FfHits => FfHits,
                    PlayerStats.TankDamage => TankDamage,
                    PlayerStats.WitchDamage => WitchDamage,
                    PlayerStats.MeleesOnTank => MeleesOnTank,
                    PlayerStats.RockSkeets => RockSkeets,
                    PlayerStats.RockEats => RockEats,
                    PlayerStats.FfGivenPellet => FfGivenPellet,
                    PlayerStats.FfGivenBullet => FfGivenBullet,
                    PlayerStats.FfGivenSniper => FfGivenSniper,
                    PlayerStats.FfGivenMelee => FfGivenMelee,
                    PlayerStats.FfGivenFire => FfGivenFire,
                    PlayerStats.FfGivenIncap => FfGivenIncap,
                    PlayerStats.FfGivenOther => FfGivenOther,
                    PlayerStats.FfGivenSelf => FfGivenSelf,
                    PlayerStats.FfTakenPellet => FfTakenPellet,
                    PlayerStats.FfTakenBullet => FfTakenBullet,
                    PlayerStats.FfTakenSniper => FfTakenSniper,
                    PlayerStats.FfTakenMelee => FfTakenMelee,
                    PlayerStats.FfTakenFire => FfTakenFire,
                    PlayerStats.FfTakenIncap => FfTakenIncap,
                    PlayerStats.FfTakenOther => FfTakenOther,
                    PlayerStats.FfGivenTotal => FfGivenTotal,
                    PlayerStats.FfTakenTotal => FfTakenTotal,
                    PlayerStats.Clears => Clears,
                    PlayerStats.AvgClearTime => AvgClearTime,
                    PlayerStats.TimeStartPresent => TimeStartPresent.ToUnixTimeSeconds(),
                    PlayerStats.TimeStopPresent => TimeStopPresent.ToUnixTimeSeconds(),
                    PlayerStats.TimeStartAlive => TimeStartAlive.ToUnixTimeSeconds(),
                    PlayerStats.TimeStopAlive => TimeStopAlive.ToUnixTimeSeconds(),
                    PlayerStats.TimeStartUpright => TimeStartUpright.ToUnixTimeSeconds(),
                    PlayerStats.TimeStopUpright => TimeStopUpright.ToUnixTimeSeconds(),
                    _ => throw new ArgumentOutOfRangeException(nameof(playerStats), playerStats, null)
                };
            }
        }
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
        public int Ledged { get; set; }
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

        public long this[InfectedStats infectedStats]
        {
            get
            {
                return infectedStats switch
                {
                    InfectedStats.DmgTotal => DmgTotal,
                    InfectedStats.DmgUpright => DmgUpright,
                    InfectedStats.DmgTank => DmgTank,
                    InfectedStats.DmgTankIncap => DmgTankIncap,
                    InfectedStats.DmgScratch => DmgScratch,
                    InfectedStats.DmgSpit => DmgSpit,
                    InfectedStats.DmgBoom => DmgBoom,
                    InfectedStats.DmgTankUp => DmgTankUp,
                    InfectedStats.HunterDPs => HunterDPs,
                    InfectedStats.HunterDpDmg => HunterDpDmg,
                    InfectedStats.JockeyDPs => JockeyDPs,
                    InfectedStats.DeathCharges => DeathCharges,
                    InfectedStats.Booms => Booms,
                    InfectedStats.Ledged => Ledged,
                    InfectedStats.Common => Common,
                    InfectedStats.Spawns => Spawns,
                    InfectedStats.SpawnSmoker => SpawnSmoker,
                    InfectedStats.SpawnBoomer => SpawnBoomer,
                    InfectedStats.SpawnHunter => SpawnHunter,
                    InfectedStats.SpawnCharger => SpawnCharger,
                    InfectedStats.SpawnSpitter => SpawnSpitter,
                    InfectedStats.SpawnJockey => SpawnJockey,
                    InfectedStats.TankPasses => TankPasses,
                    InfectedStats.TimeStartPresent => TimeStartPresent.ToUnixTimeSeconds(),
                    InfectedStats.TimeStopPresent => TimeStopPresent.ToUnixTimeSeconds(),
                    _ => throw new ArgumentOutOfRangeException(nameof(infectedStats), infectedStats, null)
                };
            }
        }
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