﻿namespace L4D2PlayStats.GameInfo.Models;

public class Scoreboard
{
    public int SurvivorScore { get; set; }
    public int InfectedScore { get; set; }
    public int Bonus { get; set; }
    public int MaxBonus { get; set; }
    public float CurrentProgress { get; private set; }
    public int Difference => SurvivorScore - InfectedScore;
    public int Comeback => Math.Abs(Difference);
    public bool IsSurvivorsWinning => SurvivorScore > InfectedScore;
    public bool IsInfectedsWinning => InfectedScore > SurvivorScore;
    public bool IsDraw => SurvivorScore == InfectedScore;

    public void UpdateCurrentProgress(Survivor[] survivors)
    {
        CurrentProgress = survivors.Select(s => s.Progress).DefaultIfEmpty(0).Max();
    }
}