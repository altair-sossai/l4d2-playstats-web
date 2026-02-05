namespace L4D2PlayStats.Core.UserAvatar;

public interface IUserAvatar
{
    string this[long communityId] { get; }
    string this[string? communityId] { get; }
    Task LoadAsync(long communityId, bool fireAndForget = true, CancellationToken cancellationToken = default);
    Task LoadAsync(string communityId, bool fireAndForget = true, CancellationToken cancellationToken = default);
    Task LoadAsync(IEnumerable<long> communityIds, bool fireAndForget = true, CancellationToken cancellationToken = default);
    Task LoadAsync(IEnumerable<string?> communityIds, bool fireAndForget = true, CancellationToken cancellationToken = default);
}