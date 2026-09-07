using System.Collections.Concurrent;
using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Filters;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.GameInfo.Models.Events;
using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;
using L4D2PlayStats.Core.GameInfo.Results;
using L4D2PlayStats.Core.GameInfo.Structures;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.UserAvatar;
using Serilog;

namespace L4D2PlayStats.Core.GameInfo;

public class GameInfo
{
#if DEBUG
    private static readonly TimeSpan GlobalMessageCooldown = TimeSpan.FromMilliseconds(300);
    private static readonly TimeSpan UserMessageCooldown = TimeSpan.FromMilliseconds(300);
#else
    private static readonly TimeSpan GlobalMessageCooldown = TimeSpan.FromSeconds(15);
    private static readonly TimeSpan UserMessageCooldown = TimeSpan.FromSeconds(30);
#endif

    private static readonly TimeSpan MessageRetention = TimeSpan.FromHours(1);
    private static readonly TimeSpan FeedRetention = TimeSpan.FromHours(1);
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(15);

    private static readonly Lock Lock = new();

    private static GameInfo? _gameInfo;
    private readonly TimedValue<Configuration?> _configuration = new(Delay, TimeSpan.FromHours(2));
    private readonly List<ExternalChatMessage> _externalMessages = [];
    private readonly TimedFeed _feed = new(Delay, FeedRetention);
    private readonly TimedValue<Infected[]> _infecteds = new(Delay, TimeSpan.FromHours(2), []);
    private readonly ConcurrentDictionary<string, string> _lastMessage = new();
    private readonly TimedValue<Round?> _round = new(Delay, TimeSpan.FromHours(2));
    private readonly TimedValue<Scoreboard?> _scoreboard = new(Delay, TimeSpan.FromMinutes(10));
    private readonly TimedValue<Models.Player[]> _spectators = new(Delay, TimeSpan.FromHours(2), []);
    private readonly TimedValue<Survivor[]> _survivors = new(Delay, TimeSpan.FromHours(2), []);
    private readonly IUserAvatar _userAvatar;

    public readonly Queue<ServerCommand> ServerCommands = new();

    private GameInfo(IUserAvatar userAvatar)
    {
        _userAvatar = userAvatar;

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

    public Models.Player[] Spectators
    {
        get => _spectators;
        set => _spectators.Value = value;
    }

    public bool AnyPlayerConnected => Survivors.Length > 0 || Infecteds.Length > 0 || Spectators.Length > 0;

    public IReadOnlyCollection<FeedItem> Feed => _feed.Items;
    public IReadOnlyCollection<ChatMessage> Messages => [.. _feed.Items.OfType<ChatMessage>()];
    public IReadOnlyCollection<GameEvent> Events => [.. _feed.Items.OfType<GameEvent>()];
    public IReadOnlyCollection<ExternalChatMessage> ExternalMessages => [.. _externalMessages.Where(w => w.Age < MessageRetention)];

    public IReadOnlyCollection<FeedItem> After(long after)
    {
        return [.. Feed.Where(item => item.Ticks > after)];
    }

    public static GameInfo GetOrInitializeInstance(IUserAvatar userAvatar)
    {
        if (_gameInfo != null)
            return _gameInfo;

        lock (Lock)
        {
            return _gameInfo ??= new GameInfo(userAvatar);
        }
    }

    public void AddMessage(ChatMessageCommand command)
    {
        if (string.IsNullOrEmpty(command.CommunityId) || string.IsNullOrEmpty(command.Message))
            return;

        if (ChatBindBlacklist.IsBind(command.Message))
            return;

        if (_lastMessage.TryGetValue(command.CommunityId, out var last) && last.Equals(command.Message, StringComparison.CurrentCultureIgnoreCase))
            return;

        _lastMessage[command.CommunityId] = command.Message;

        _feed.Add(new ChatMessage(command));
    }

    public void AddEvent(GameEvent gameEvent)
    {
        gameEvent.When = DateTime.UtcNow;

        _feed.Add(gameEvent);
    }

    public SendExternalMessageResult AddExternalMessage(User? user, ExternalChatMessageCommand? command)
    {
        if (user == null)
            return SendExternalMessageResult.FailureResult("User cannot be null.");

        if (command == null)
            return SendExternalMessageResult.FailureResult("Command cannot be null.");

        if (string.IsNullOrEmpty(user.SteamId))
            return SendExternalMessageResult.FailureResult("User SteamId cannot be null or empty.");

        if (string.IsNullOrEmpty(user.Name))
            return SendExternalMessageResult.FailureResult("User Name cannot be null or empty.");

        if (string.IsNullOrEmpty(command.Message))
            return SendExternalMessageResult.FailureResult("Message cannot be null or empty.");

        if (command.Message.Length > 200)
            return SendExternalMessageResult.FailureResult("Message cannot be longer than 200 characters.");

        lock (Lock)
        {
            _externalMessages.RemoveAll(m => m.Age >= MessageRetention);

            if (!user.IsAdmin)
            {
                var lastMessage = _externalMessages.LastOrDefault();
                if (lastMessage != null && lastMessage.Age < GlobalMessageCooldown)
                    return SendExternalMessageResult.FailureResult("Max message rate exceeded. Please wait.");

                var lastUserMessage = _externalMessages.LastOrDefault(m => m.SteamId == user.SteamId);
                if (lastUserMessage != null && lastUserMessage.Age < UserMessageCooldown)
                    return SendExternalMessageResult.FailureResult("Max message rate exceeded. Please wait.");
            }

            var message = new ExternalChatMessage(user, command);

            _externalMessages.Add(message);
            _feed.Add((ChatMessage)message, false);
        }

        return SendExternalMessageResult.SuccessResult();
    }

    private void SurvivorsValueUpdated(object? sender, Survivor[] survivors)
    {
        Array.Sort(survivors, (a, b) => a.Character.CompareTo(b.Character));

        _ = LoadAvatarAsync(survivors);
    }

    private void InfectedsValueUpdated(object? sender, Infected[] infecteds)
    {
        Array.Sort(infecteds, (a, b) => a.Damage.CompareTo(b.Damage));

        _ = LoadAvatarAsync(infecteds);
    }

    private static void SpectatorsValueUpdated(object? sender, Models.Player[] players)
    {
        Array.Sort(players, (a, b) => a.Name?.CompareTo(b.Name) ?? 0);
    }

    private async Task LoadAvatarAsync(IReadOnlyCollection<Models.Player> players)
    {
        try
        {
            var communityIds = players.Select(p => p.CommunityId);

            await _userAvatar.LoadAsync(communityIds);

            foreach (var player in players)
                player.UpdateAvatarUrl(_userAvatar);
        }
        catch (Exception exception)
        {
            Log.Error(exception, "An error occurred while loading avatars.");
        }
    }
}