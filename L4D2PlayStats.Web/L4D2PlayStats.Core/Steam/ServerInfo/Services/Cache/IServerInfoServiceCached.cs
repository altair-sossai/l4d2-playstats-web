using L4D2PlayStats.Core.Steam.ServerInfo.Responses;

namespace L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;

public interface IServerInfoServiceCached
{
    Task<GetServerListResponse?> GetServerInfoAsync(string key, string filter, CancellationToken cancellationToken);
}