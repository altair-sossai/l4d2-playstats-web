using L4D2PlayStats.Core.Steam.ServerInfo.Responses;

namespace L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;

public interface IServerInfoServiceCached
{
    Task<GetServerListResponse.ServerInfo?> GetServerInfoAsync(CancellationToken cancellationToken);
}