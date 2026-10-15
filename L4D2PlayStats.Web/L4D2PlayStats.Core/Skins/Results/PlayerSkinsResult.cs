namespace L4D2PlayStats.Core.Skins.Results;

public class PlayerSkinsResult
{
    public string CommunityId { get; init; } = string.Empty;
    public HashSet<string> Owned { get; init; } = [];
    public Dictionary<string, string> Equipped { get; init; } = [];

    public bool Owns(string skinId)
    {
        return Owned.Contains(skinId);
    }

    public bool IsEquipped(Skin skin)
    {
        return Equipped.TryGetValue(skin.Weapon, out var skinId) && skinId == skin.Id;
    }
}