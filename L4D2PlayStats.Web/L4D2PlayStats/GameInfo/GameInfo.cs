using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.GameInfo.Models;
using L4D2PlayStats.Infrastructure.Structures;

namespace L4D2PlayStats.GameInfo;

public class GameInfo
{
    private readonly TimedValue<bool> _areTeamsFlipped = new();
    private readonly TimedValue<string?> _configurationName = new();
    private readonly TimedValue<Infected[]> _infecteds = new([]);
    private readonly TimedValue<int> _maxChapterProgressPoints = new();
    private readonly TimedList<ChatMessage> _messages = new();
    private readonly TimedValue<Player[]> _spectators = new([]);
    private readonly TimedValue<Survivor[]> _survivors = new([]);
    private readonly TimedValue<int> _teamSize = new();

    public GameInfo()
    {
        _messages.ItemAdded += (_, _) => _messages.Items.Sort((a, b) => a.When.CompareTo(b.When));
    }

    public bool AreTeamsFlipped
    {
        get => _areTeamsFlipped;
        private set => _areTeamsFlipped.Value = value;
    }

    public int Round => AreTeamsFlipped ? 2 : 1;

    public int MaxChapterProgressPoints
    {
        get => _maxChapterProgressPoints;
        set => _maxChapterProgressPoints.Value = value;
    }

    public int TeamSize
    {
        get => _teamSize;
        set => _teamSize.Value = value;
    }

    public string? ConfigurationName
    {
        get => _configurationName;
        set => _configurationName.Value = value;
    }

    public Scoreboard Scoreboard { get; } = new();


    public Survivor[] Survivors
    {
        get => _survivors;
        set => _survivors.Value = value;
    }

    public Infected[] Infecteds
    {
        get => _infecteds;
        set => _infecteds.Value = value;
    }

    public Player[] Spectators
    {
        get => _spectators;
        set => _spectators.Value = value;
    }

    public IReadOnlyCollection<ChatMessage> Messages => _messages.Items;

    public void Update(GameInfoCommand command)
    {
        AreTeamsFlipped = command.AreTeamsFlipped == 1;
        MaxChapterProgressPoints = command.MaxChapterProgressPoints;
        TeamSize = command.TeamSize;
        ConfigurationName = command.ConfigurationName;

        Scoreboard.Update(command);
    }

    public void UpdatePlayers(PlayersCommand command)
    {
        Survivors = command.Survivors?.ToArray() ?? [];
        Infecteds = command.Infecteds?.ToArray() ?? [];
        Spectators = command.Spectators?.ToArray() ?? [];
    }

    public void AddMessage(ChatMessageCommand command)
    {
        var message = new ChatMessage(command);

        _messages.Add(message);
    }
}