namespace L4D2PlayStats.Core.Auth;

public interface ICurrentUser
{
    public bool IsAuthenticated { get; }
    public bool IsAdmin { get; }
    public string? SteamId { get; }
    public string? CommunityId { get; }
    public string? Steam3 { get; }
    public string? ProfileUrl { get; }
    public string? Name { get; }
    public string? AvatarUrl { get; }
    public bool ItIsMe(long? communityId);
    public bool ItIsMe(string? communityId);
}