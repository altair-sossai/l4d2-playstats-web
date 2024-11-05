﻿namespace L4D2PlayStats.GameInfo.Models;

public class Scoreboard
{
    public int SurvivorScore { get; set; }
    public int InfectedScore { get; set; }
    public int Difference => SurvivorScore - InfectedScore;
    public int Comeback => Math.Abs(Difference);
    public bool IsSurvivorsWinning => SurvivorScore > InfectedScore;
    public bool IsInfectedsWinning => InfectedScore > SurvivorScore;
    public bool IsDraw => SurvivorScore == InfectedScore;
}