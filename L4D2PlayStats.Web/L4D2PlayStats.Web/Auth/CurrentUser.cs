using System.Text.RegularExpressions;
using L4D2PlayStats.Auth;
using L4D2PlayStats.Infrastructure.Structures;
using L4D2PlayStats.Steam.SteamUser.Responses;
using L4D2PlayStats.Steam.SteamUser.Services;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Web.Auth;

public class CurrentUser : ICurrentUser
{
    private const string Pattern = @"https:\/\/steamcommunity\.com\/openid\/id\/(\d+)";
    private static readonly Regex Regex = new(Pattern);
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly SteamIdentifiers _steamIdentifiers;
    private readonly ISteamUserService _steamUserService;
    private readonly User? _user;

    public CurrentUser(IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        IMemoryCache memoryCache,
        ISteamUserService steamUserService)
    {
        _configuration = configuration;
        _steamUserService = steamUserService;
        _memoryCache = memoryCache;

        IsAuthenticated = httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        if (!IsAuthenticated)
            return;

        CommunityId = ParseCommunityId(httpContextAccessor);
        if (string.IsNullOrEmpty(CommunityId))
            return;

        _user = GetUserAsync().Result;
    }

    private string SteamApiKey => _configuration.GetValue<string>("SteamApiKey")!;

    public bool IsAuthenticated { get; }

    public string? CommunityId
    {
        get => _steamIdentifiers.CommunityId?.ToString();
        private init => SteamIdentifiers.TryParse(value, out _steamIdentifiers);
    }

    public string? SteamId => _steamIdentifiers.SteamId;
    public string? Steam3 => _steamIdentifiers.Steam3;
    public string? ProfileUrl => _steamIdentifiers.ProfileUrl;
    public string? Name => _user?.Name;
    public string? AvatarUrl => _user?.AvatarUrl;

    private Task<User?> GetUserAsync()
    {
        if (string.IsNullOrEmpty(CommunityId))
            return Task.FromResult(null as User);

        return _memoryCache.GetOrCreateAsync($"CurrentUser_{CommunityId}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(4);

            var response = await _steamUserService.GetPlayerSummariesAsync(SteamApiKey, CommunityId);
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
        public string? AvatarUrl { get; } = playerInfo.AvatarFull?.Replace("https://", "http://");
    }
}