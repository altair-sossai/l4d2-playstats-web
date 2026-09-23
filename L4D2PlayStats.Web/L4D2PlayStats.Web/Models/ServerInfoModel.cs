using L4D2PlayStats.Core.GameInfo;
using static L4D2PlayStats.Core.Steam.ServerInfo.Responses.GetServerListResponse;

namespace L4D2PlayStats.Web.Models;

public class ServerInfoModel(
    string serverIp,
    string serverDns,
    GameInfo gameInfo,
    ServerInfo? serverInfo)
{
    public string ServerIp { get; } = serverIp;
    public string ServerDns { get; } = serverDns;
    public GameInfo GameInfo { get; } = gameInfo;
    public ServerInfo? ServerInfo { get; } = serverInfo;
    public bool AnyPlayerConnected => ServerInfo?.Players > 0;
}