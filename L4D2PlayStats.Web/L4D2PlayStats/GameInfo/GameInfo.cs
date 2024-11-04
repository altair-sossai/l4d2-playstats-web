using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.GameInfo.Models;
using L4D2PlayStats.Infrastructure.Structures;

namespace L4D2PlayStats.GameInfo;

public class GameInfo
{
    private readonly TimedValue<Configuration?> _configuration = new(expireIn: TimeSpan.FromDays(1));
    private readonly TimedValue<Infected[]> _infecteds = new([]);
    private readonly TimedList<ChatMessage> _messages = new();
    private readonly TimedValue<Round?> _round = new();
    private readonly TimedValue<Scoreboard?> _scoreboard = new();
    private readonly TimedValue<Player[]> _spectators = new([]);
    private readonly TimedValue<Survivor[]> _survivors = new([]);

    public GameInfo()
    {
        _messages.ItemAdded += (_, _) => _messages.Items.Sort((a, b) => a.When.CompareTo(b.When));
    }

    public Configuration? Configuration
    {
        get => _configuration;
        set => _configuration.Value = value;
    }

    public Round? Round
    {
        get => _round;
        set => _round.Value = value;
    }

    public Scoreboard? Scoreboard
    {
        get => _scoreboard;
        set => _scoreboard.Value = value;
    }

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

    public void AddMessage(ChatMessageCommand command)
    {
        var message = new ChatMessage(command);

        _messages.Add(message);
    }
}