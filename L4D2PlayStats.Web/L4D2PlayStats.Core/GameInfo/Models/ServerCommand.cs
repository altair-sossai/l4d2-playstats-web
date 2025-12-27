using System.Text.RegularExpressions;

namespace L4D2PlayStats.Core.GameInfo.Models;

public class ServerCommand
{
    private static readonly Regex Part = new(@"^(!|/|sm_)(\w+)( (.+))?$");

    private ServerCommand(string message)
    {
        var match = Part.Match(message);

        if (!match.Success)
            throw new ArgumentException("Invalid command format", nameof(message));

        Prefix = match.Groups[1].Value;
        Command = match.Groups[2].Value;
        Arguments = match.Groups[4].Success ? match.Groups[4].Value : string.Empty;
    }

    public string Prefix { get; }
    public string Arguments { get; }
    public string Command { get; }
    public string FullCommand => $"sm_{Command} {Arguments}".Trim();

    public override string ToString()
    {
        return FullCommand;
    }

    public static bool TryParse(string? message, out ServerCommand? serverCommand)
    {
        if (!string.IsNullOrEmpty(message) && IsCommand(message))
        {
            serverCommand = new ServerCommand(message);
            return true;
        }

        serverCommand = null;

        return false;
    }

    public static bool IsCommand(string? message)
    {
        if (string.IsNullOrEmpty(message) || message.Length > 256)
            return false;

        return Part.IsMatch(message);
    }
}