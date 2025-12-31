using System.Text.RegularExpressions;
using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Steam.SteamUser.Responses;
using L4D2PlayStats.Core.Steam.SteamUser.Services;
using L4D2PlayStats.Core.UserAvatar;
using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Retry;

namespace L4D2PlayStats.Web.Auth;

public class CurrentUser : ICurrentUser
{
    private const string Pattern = @"https:\/\/steamcommunity\.com\/openid\/id\/(\d+)";
    private static readonly Regex Regex = new(Pattern);
    private readonly IMemoryCache _memoryCache;

    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(3, attempt)), (exception, _, retryCount, _) => { Console.WriteLine($"Retry {retryCount} due to: {exception.Message}"); });

    private readonly SteamIdentifiers _steamIdentifiers;
    private readonly ISteamUserService _steamUserService;
    private readonly User? _user;
    private readonly IUserAvatar _userAvatar;

    public CurrentUser(IAppOptionsWraper config,
        IHttpContextAccessor httpContextAccessor,
        IMemoryCache memoryCache,
        ISteamUserService steamUserService,
        IUserAvatar userAvatar)
    {
        _memoryCache = memoryCache;
        _steamUserService = steamUserService;
        _userAvatar = userAvatar;

        SteamApiKey = new Lazy<string>(() => config.SteamApiKey);
        ServerAdmins = new Lazy<string[]>(() => config.ServerAdmins);

        IsAuthenticated = httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        if (!IsAuthenticated)
            return;

        CommunityId = ParseCommunityId(httpContextAccessor);
        if (string.IsNullOrEmpty(CommunityId))
            return;

        _user = GetUser();
    }

    private Lazy<string> SteamApiKey { get; }
    private Lazy<string[]> ServerAdmins { get; }

    public bool IsAuthenticated { get; }

    public bool IsAdmin => IsAuthenticated && !string.IsNullOrEmpty(CommunityId) && ServerAdmins.Value.Contains(CommunityId);

    public string? CommunityId
    {
        get => _steamIdentifiers.CommunityId?.ToString();
        private init => SteamIdentifiers.TryParse(value, out _steamIdentifiers);
    }

    public string? SteamId => _steamIdentifiers.SteamId;
    public string? Steam3 => _steamIdentifiers.Steam3;
    public string? ProfileUrl => _steamIdentifiers.ProfileUrl;
    public string? Name => _user?.Name;

    public bool ItIsMe(long? communityId)
    {
        return ItIsMe(communityId?.ToString());
    }

    public bool ItIsMe(string? communityId)
    {
        if (!IsAuthenticated || string.IsNullOrEmpty(CommunityId) || string.IsNullOrEmpty(communityId))
            return false;

        return CommunityId.Equals(communityId, StringComparison.CurrentCultureIgnoreCase);
    }

    private User? GetUser()
    {
        return GetUserAsync()
            .ConfigureAwait(false)
            .GetAwaiter()
            .GetResult();
    }

    private async Task<User?> GetUserAsync()
    {
        if (string.IsNullOrEmpty(CommunityId))
            return null;

        await _userAvatar.LoadAsync(CommunityId);

        return await _memoryCache.GetOrCreateAsync($"CurrentUser_{CommunityId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);

            var response = await _retryPolicy.ExecuteAsync(() => _steamUserService.GetPlayerSummariesAsync(SteamApiKey.Value, CommunityId));

            var responsePlayers = response?.Response?.Players;
            if (responsePlayers == null || responsePlayers.Count == 0)
                return null;

            var playerInfo = responsePlayers.First();

            return new User(playerInfo);
        });
    }

    private static string? ParseCommunityId(IHttpContextAccessor httpContextAccessor)
    {
        var claims = httpContextAccessor.HttpContext?.User.Claims;
        var claim = claims?.FirstOrDefault(c => Regex.IsMatch(c.Value));

        if (claim == null)
            return null;

        var match = Regex.Match(claim.Value);

        return match.Groups[1].Value;
    }

    private class User(GetPlayerSummariesResponse.PlayerInfo playerInfo)
    {
        public string? Name { get; } = playerInfo.PersonaName;
    }
}