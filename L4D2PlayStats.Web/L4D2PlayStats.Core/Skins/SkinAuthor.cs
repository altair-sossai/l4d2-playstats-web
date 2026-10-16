namespace L4D2PlayStats.Core.Skins;

public record SkinAuthor
{
    public string Name { get; init; } = string.Empty;
    public string CommunityId { get; init; } = string.Empty;
    public string ProfileUrl => $"https://steamcommunity.com/profiles/{CommunityId}";
}