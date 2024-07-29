using L4D2PlayStats.Steam.ServerInfo.Responses;
using Refit;

namespace L4D2PlayStats.Steam.ServerInfo.Services;

public interface IServerInfoService
{
    [Get("/IGameServersService/GetServerList/v1")]
    Task<GetServerListResponse> GetServerInfo([AliasAs("key")] string key, [AliasAs("filter")] string filter);
}