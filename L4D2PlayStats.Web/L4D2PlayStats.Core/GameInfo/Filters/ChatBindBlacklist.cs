using System.Text;

namespace L4D2PlayStats.Core.GameInfo.Filters;

public static class ChatBindBlacklist
{
    private static readonly string[] Phrases =
    [
        "gogogo",
        "go go go",

        "back back",
        "back back back",

        "tank tank",
        "tank tank tank",

        "hit hit",
        "hit hit hit",

        "r"
    ];

    private static readonly HashSet<string> Normalized = BuildNormalizedSet();

    public static bool IsBind(string? message)
    {
        return !string.IsNullOrWhiteSpace(message)
               && Normalized.Contains(Normalize(message));
    }

    private static HashSet<string> BuildNormalizedSet()
    {
        var set = new HashSet<string>();

        foreach (var phrase in Phrases)
        {
            var normalized = Normalize(phrase);
            if (normalized.Length > 0)
                set.Add(normalized);
        }

        return set;
    }

    private static string Normalize(string value)
    {
        var builder = new StringBuilder(value.Length);
        var pendingSpace = false;

        foreach (var ch in value.ToLowerInvariant())
            if (char.IsLetterOrDigit(ch))
            {
                if (pendingSpace && builder.Length > 0)
                    builder.Append(' ');

                builder.Append(ch);
                pendingSpace = false;
            }
            else
                pendingSpace = true;

        return builder.ToString();
    }
}