using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.GameInfo.Results;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.UserAvatar;

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

    private static readonly Lock Lock = new();
    private static GameInfo? _gameInfo;
    private readonly TimedValue<Configuration?> _configuration = new(expireIn: TimeSpan.FromDays(1));
    private readonly List<ExternalChatMessage> _externalMessages = [];
    private readonly TimedValue<Infected[]> _infecteds = new([], TimeSpan.FromHours(2));
    private readonly Dictionary<string, string> _lastMessage = new();
    private readonly TimedList<ChatMessage> _messages = new(delay: TimeSpan.FromSeconds(10));
    private readonly TimedValue<Round?> _round = new(expireIn: TimeSpan.FromHours(2));
    private readonly TimedValue<Scoreboard?> _scoreboard = new();
    private readonly TimedValue<Models.Player[]> _spectators = new([], TimeSpan.FromHours(2));
    private readonly TimedValue<Survivor[]> _survivors = new([], TimeSpan.FromHours(2));
    private readonly IUserAvatar _userAvatar;

    public readonly Queue<ServerCommand> ServerCommands = new();

    private GameInfo(IUserAvatar userAvatar)
    {
        _userAvatar = userAvatar;

        _survivors.ValueUpdated += SurvivorsValueUpdated;
        _infecteds.ValueUpdated += InfectedsValueUpdated;
        _spectators.ValueUpdated += SpectatorsValueUpdated;

        _messages.ItemAdded += MessagesItemAdded;
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

    public IReadOnlyCollection<ChatMessage> Messages => _messages.Items;
    public IReadOnlyCollection<ExternalChatMessage> ExternalMessages => _externalMessages.Where(w => w.Age < MessageRetention).ToList();

    public IReadOnlyCollection<ChatMessage> AllMessages => Messages
        .Concat(ExternalMessages.Select(em => (ChatMessage)em))
        .OrderBy(m => m.When)
        .ToList();

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

        if (_lastMessage.ContainsKey(command.CommunityId) && _lastMessage[command.CommunityId].Equals(command.Message, StringComparison.CurrentCultureIgnoreCase))
            return;

        _lastMessage[command.CommunityId] = command.Message;

        var message = new ChatMessage(command);

        _messages.Add(message);
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

            var lastMessage = _externalMessages.LastOrDefault();
            if (lastMessage != null && lastMessage.Age < GlobalMessageCooldown && !user.IsAdmin)
                return SendExternalMessageResult.FailureResult("Max message rate exceeded. Please wait.");

            var lastUserMessage = _externalMessages.LastOrDefault(m => m.SteamId == user.SteamId);
            if (lastUserMessage != null && lastUserMessage.Age < UserMessageCooldown && !user.IsAdmin)
                return SendExternalMessageResult.FailureResult("Max message rate exceeded. Please wait.");

            var message = new ExternalChatMessage(user, command);

            _externalMessages.Add(message);
        }

        return SendExternalMessageResult.SuccessResult();
    }

    private void SurvivorsValueUpdated(object? sender, Survivor[] survivors)
    {
        Array.Sort(survivors, (a, b) => a.Character.CompareTo(b.Character));

        LoadAvatarAsync(survivors).Wait();
    }

    private void InfectedsValueUpdated(object? sender, Infected[] infecteds)
    {
        Array.Sort(infecteds, (a, b) => a.Damage.CompareTo(b.Damage));

        LoadAvatarAsync(infecteds).Wait();
    }

    private static void SpectatorsValueUpdated(object? sender, Models.Player[] players)
    {
        Array.Sort(players, (a, b) => a.Name?.CompareTo(b.Name) ?? 0);
    }

    private void MessagesItemAdded(object? sender, ChatMessage chatMessage)
    {
        _messages.Items.Sort((a, b) => a.When.CompareTo(b.When));
    }

    private async Task LoadAvatarAsync(IReadOnlyCollection<Models.Player> players)
    {
        var communityIds = players.Select(p => p.CommunityId);

        await _userAvatar.LoadAsync(communityIds);

        foreach (var player in players)
            player.UpdateAvatarUrl(_userAvatar);
    }
}