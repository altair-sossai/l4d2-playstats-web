namespace L4D2PlayStats.Core.UserAvatar;

public interface IUserAvatar
{
    string this[long communityId] { get; }
    string this[string? communityId] { get; }
    Task LoadAsync(params long[] communityIds);
    Task LoadAsync(IEnumerable<long> communityIds);
    Task LoadAsync(IEnumerable<string?> communityIds);
}