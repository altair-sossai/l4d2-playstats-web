﻿namespace L4D2PlayStats.Core.GameInfo.Models;

public class Scoreboard
{
    public int SurvivorScore { get; set; }
    public int InfectedScore { get; set; }
    public int Bonus { get; set; }
    public int MaxBonus { get; set; }
    public decimal CurrentProgress { get; set; }
    public int CurrentProgressPoints { get; set; }
    public bool IsTankInPlay { get; set; }
    public bool TankIsDead { get; set; }
    public decimal BonusPercentage => MaxBonus == 0 ? 0 : Bonus / (decimal)MaxBonus;
    public int Difference => SurvivorScore - InfectedScore;
    public int Comeback => Math.Abs(Difference);
    public bool IsSurvivorsWinning => SurvivorScore > InfectedScore;
    public bool IsInfectedsWinning => InfectedScore > SurvivorScore;
    public bool IsDraw => SurvivorScore == InfectedScore;
}