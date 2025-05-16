using System.Text.RegularExpressions;
using L4D2PlayStats.Core.Infrastructure.Extensions;

namespace L4D2PlayStats.Core.Infrastructure.Helpers;

public static class SteamIdHelper
{
    private const string SteamIdPattern = @"^STEAM_\d:(\d):(\d+)$";
    private const string Steam3Pattern = @"^\[?U:\d:(\d+)]?$";

    private const long MagicNumber = 76561197960265728;

    private static readonly Regex SteamIdRegex = new(SteamIdPattern);
    private static readonly Regex Steam3Regex = new(Steam3Pattern);

    public static long? SteamIdToCommunityId(string value)
    {
        var match = SteamIdRegex.Match(value);
        if (!match.Success)
            return null;

        var authServer = long.Parse(match.Groups[1].Value);
        var authid = long.Parse(match.Groups[2].Value);

        return authid * 2 + MagicNumber + authServer;
    }

    public static long? Steam3ToCommunityId(string value)
    {
        var match = Steam3Regex.Match(value);

        if (!match.Success)
            return null;

        var steam3 = long.Parse(match.Groups[1].Value);

        return steam3 + MagicNumber;
    }

    public static long? ParseCommunityId(string value)
    {
        var patterns = new[]
        {
            @"^(\d{16,})$",
            @"^https?:\/\/steamcommunity\.com\/profiles\/(\d{16,})\/?$"
        };

        var matchValue = value.MatchValue(patterns);

        return long.TryParse(matchValue, out var communityId) ? communityId : null;
    }

    public static string? CommunityIdToSteamId(long communityId)
    {
        if (communityId <= 0)
            return null;

        var authserver = (communityId - MagicNumber) & 1;
        var authid = (communityId - MagicNumber - authserver) / 2;

        return $"STEAM_0:{authserver}:{authid}";
    }

    public static string? CommunityIdToSteam3(long communityId)
    {
        if (communityId <= 0)
            return null;

        var authserver = communityId - MagicNumber;

        return $"[U:1:{authserver}]";
    }

    public static string? ProfileUrl(long communityId)
    {
        return communityId <= 0 ? null : $"https://steamcommunity.com/profiles/{communityId}";
    }
}