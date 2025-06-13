using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string TruncatePlayerName(this string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        if (name.Length >= 15 && !name.Contains(' '))
            return name.Truncate(10).Trim();

        return name.Truncate(30).Trim();
    }

    public static string Truncate(this string value, int max)
    {
        return value.Length <= max ? value : value[..max];
    }

    public static string? MatchValue(this string input, IEnumerable<string> patterns)
    {
        if (string.IsNullOrEmpty(input))
            return null;

        var pattern = patterns.FirstOrDefault(pattern => Regex.IsMatch(input, pattern));

        if (string.IsNullOrEmpty(pattern))
            return null;

        var match = Regex.Match(input, pattern);
        var group = match.Groups[1];

        return group.Value;
    }
}