using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.GameInfo.Models;
using L4D2PlayStats.Infrastructure.Structures;

namespace L4D2PlayStats.GameInfo;

public class GameInfo
{
    public static readonly GameInfo Current = new();

    private readonly TimedValue<Configuration?> _configuration = new(expireIn: TimeSpan.FromDays(1));
    private readonly TimedValue<Infected[]> _infecteds = new([], TimeSpan.FromHours(2));
    private readonly TimedList<ChatMessage> _messages = new();
    private readonly TimedValue<Round?> _round = new(expireIn: TimeSpan.FromHours(2));
    private readonly TimedValue<Scoreboard?> _scoreboard = new();
    private readonly TimedValue<Player[]> _spectators = new([], TimeSpan.FromHours(2));
    private readonly TimedValue<Survivor[]> _survivors = new([], TimeSpan.FromHours(2));

    private GameInfo()
    {
        _messages.ItemAdded += MessagesItemAdded;
        _survivors.ValueUpdated += SurvivorsValueUpdated;
        _infecteds.ValueUpdated += InfectedsValueUpdated;
        _spectators.ValueUpdated += SpectatorsValueUpdated;
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

    private void MessagesItemAdded(object? sender, ChatMessage chatMessage)
    {
        _messages.Items.Sort((a, b) => a.When.CompareTo(b.When));
    }

    private void SurvivorsValueUpdated(object? sender, Survivor[] survivors)
    {
        Array.Sort(survivors, (a, b) => a.Character.CompareTo(b.Character));

        _scoreboard.Value?.UpdateCurrentProgress(survivors);
    }

    private static void InfectedsValueUpdated(object? sender, Infected[] infecteds)
    {
        Array.Sort(infecteds, (a, b) => b.Damage.CompareTo(a.Damage));
    }

    private static void SpectatorsValueUpdated(object? sender, Player[] players)
    {
        Array.Sort(players, (a, b) => a.Name?.CompareTo(b.Name) ?? 0);
    }
}