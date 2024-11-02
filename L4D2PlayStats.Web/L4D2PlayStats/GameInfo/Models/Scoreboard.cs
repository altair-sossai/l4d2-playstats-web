using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.Infrastructure.Structures;

namespace L4D2PlayStats.GameInfo.Models;

public class Scoreboard
{
    private readonly TimedValue<int> _infecteds = new();
    private readonly TimedValue<int> _survivors = new();

    public int Survivors
    {
        get => _survivors;
        set => _survivors.Value = value;
    }

    public int Infecteds
    {
        get => _infecteds;
        set => _infecteds.Value = value;
    }

    public int Difference => Survivors - Infecteds;
    public int Comeback => Math.Abs(Difference);
    public bool IsSurvivorsWinning => Survivors > Infecteds;
    public bool IsInfectedsWinning => Infecteds > Survivors;
    public bool IsDraw => Survivors == Infecteds;

    public void Update(GameInfoCommand command)
    {
        Survivors = command.SurvivorScore;
        Infecteds = command.InfectedScore;
    }
}