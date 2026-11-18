using L4D2PlayStats.Core.AntiCheatAccess.Enums;

namespace L4D2PlayStats.Core.AntiCheatAccess.Results;

public class AntiCheatAccessResult
{
    public AntiCheatAccessMode Mode { get; init; }
    public bool ModeSaved { get; init; }
    public string? UpdatedByName { get; init; }
    public string? UpdatedFrom { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public List<AntiCheatAccessPlayerResult> Whitelist { get; init; } = [];
    public List<AntiCheatAccessPlayerResult> Required { get; init; } = [];

    public List<AntiCheatAccessPlayerResult> this[AntiCheatAccessList list] => list == AntiCheatAccessList.Whitelist ? Whitelist : Required;
}
