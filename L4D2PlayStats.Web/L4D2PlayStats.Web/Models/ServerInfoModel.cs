using L4D2PlayStats.Core.GameInfo;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using static L4D2PlayStats.Core.Steam.ServerInfo.Responses.GetServerListResponse;

namespace L4D2PlayStats.Web.Models;

public class ServerInfoModel(
    string serverIp,
    GameInfo gameInfo,
    GetServerListResponse? serverListResponse)
{
    public string ServerIp { get; } = serverIp;
    public GameInfo GameInfo { get; } = gameInfo;
    public ServerInfo? ServerInfo { get; } = serverListResponse?.Response?.Servers?.FirstOrDefault();
    public bool AnyPlayerConnected => ServerInfo?.Players > 0;
}