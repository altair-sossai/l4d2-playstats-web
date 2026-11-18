using L4D2PlayStats.Core.AntiCheatAccess.Enums;

namespace L4D2PlayStats.Core.AntiCheatAccess;

public static class AntiCheatAccessNames
{
    public const string Everyone = "everyone";
    public const string Listed = "listed";
    public const string Whitelist = "whitelist";
    public const string Required = "required";

    public const AntiCheatAccessMode DefaultMode = AntiCheatAccessMode.Listed;

    public static string ToName(this AntiCheatAccessMode mode)
    {
        return mode == AntiCheatAccessMode.Everyone ? Everyone : Listed;
    }

    public static string ToName(this AntiCheatAccessList list)
    {
        return list == AntiCheatAccessList.Whitelist ? Whitelist : Required;
    }

    public static AntiCheatAccessList ListOf(this AntiCheatAccessMode mode)
    {
        return mode == AntiCheatAccessMode.Everyone ? AntiCheatAccessList.Whitelist : AntiCheatAccessList.Required;
    }

    public static bool TryParseMode(string? value, out AntiCheatAccessMode mode)
    {
        switch (value?.Trim().ToLowerInvariant())
        {
            case Everyone:
                mode = AntiCheatAccessMode.Everyone;
                return true;
            case Listed:
                mode = AntiCheatAccessMode.Listed;
                return true;
            default:
                mode = DefaultMode;
                return false;
        }
    }

    public static bool TryParseList(string? value, out AntiCheatAccessList list)
    {
        switch (value?.Trim().ToLowerInvariant())
        {
            case Whitelist:
                list = AntiCheatAccessList.Whitelist;
                return true;
            case Required:
                list = AntiCheatAccessList.Required;
                return true;
            default:
                list = AntiCheatAccessList.Required;
                return false;
        }
    }
}
