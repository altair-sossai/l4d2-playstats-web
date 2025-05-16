using System.Reflection;
using L4D2PlayStats.Core.Players.Enums;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Core.Players;

public class Player(PlayerResult playerResult, PlayerResultProperty property)
{
    private static readonly HashSet<string> Properties = Enum
        .GetValues<PlayerResultProperty>()
        .Select(e => e.ToString())
        .ToHashSet();

    private static readonly Dictionary<string, PropertyInfo> PropertiesInfo = typeof(PlayerResult)
        .GetProperties()
        .Where(p => Properties.Contains(p.Name))
        .ToDictionary(p => p.Name, p => p);

    public long CommunityId => playerResult.CommunityId;
    public string? Name => playerResult.Name;
    public decimal Value { get; } = Convert.ToDecimal(PropertiesInfo[property.ToString()].GetValue(playerResult) ?? 0);
}