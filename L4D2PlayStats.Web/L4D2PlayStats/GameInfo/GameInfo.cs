using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.GameInfo.Models;
using L4D2PlayStats.Infrastructure.Structures;

namespace L4D2PlayStats.GameInfo;

public class GameInfo
{
    private readonly TimedValue<bool> _areTeamsFlipped = new();

    public bool AreTeamsFlipped
    {
        get => _areTeamsFlipped;
        private set => _areTeamsFlipped.Value = value;
    }

    public Scoreboard Scoreboard { get; } = new();
    public int Round => AreTeamsFlipped ? 2 : 1;

    public void Update(GameInfoCommand command)
    {
        AreTeamsFlipped = command.AreTeamsFlipped == 1;

        Scoreboard.Update(command);
    }
}