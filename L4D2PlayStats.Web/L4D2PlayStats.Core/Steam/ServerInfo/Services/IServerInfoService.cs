using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using Refit;

namespace L4D2PlayStats.Core.Steam.ServerInfo.Services;

public interface IServerInfoService
{
    [Get("/IGameServersService/GetServerList/v1")]
    Task<GetServerListResponse> GetServerInfoAsync([AliasAs("key")] string key, [AliasAs("filter")] string filter, CancellationToken cancellationToken);
}