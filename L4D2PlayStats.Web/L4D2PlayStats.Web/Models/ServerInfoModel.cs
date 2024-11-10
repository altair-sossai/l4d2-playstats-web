using L4D2PlayStats.Steam.Players;
using L4D2PlayStats.Steam.ServerInfo.Responses;
using static L4D2PlayStats.Steam.ServerInfo.Responses.GetServerListResponse;

namespace L4D2PlayStats.Web.Models;

public class ServerInfoModel(
    string serverIp,
    GameInfo.GameInfo gameInfo,
    GetServerListResponse? serverListResponse,
    List<Player>? players)
{
    public string ServerIp { get; } = serverIp;
    public GameInfo.GameInfo GameInfo { get; } = gameInfo;
    public ServerInfo? ServerInfo { get; } = serverListResponse?.Response?.Servers?.FirstOrDefault();
    public List<Player>? Players { get; } = players;
}