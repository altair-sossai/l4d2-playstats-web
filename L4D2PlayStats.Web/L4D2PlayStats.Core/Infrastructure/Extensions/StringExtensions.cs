using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.Infrastructure.Extensions;

public static class StringExtensions
{
    extension(string name)
    {
        public string TruncatePlayerName()
        {
            if (string.IsNullOrEmpty(name))
                return name;

            if (name.Length >= 15 && !name.Contains(' '))
                return name.Truncate(10).Trim();

            return name.Truncate(30).Trim();
        }

        public string Truncate(int max)
        {
            return name.Length <= max ? name : name[..max];
        }

        public string? MatchValue(IEnumerable<string> patterns)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var pattern = patterns.FirstOrDefault(pattern => Regex.IsMatch(name, pattern));

            if (string.IsNullOrEmpty(pattern))
                return null;

            var match = Regex.Match(name, pattern);
            var group = match.Groups[1];

            return group.Value;
        }
    }
}