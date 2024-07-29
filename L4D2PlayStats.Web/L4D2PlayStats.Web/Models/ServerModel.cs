using L4D2PlayStats.A2S.Players;
using L4D2PlayStats.A2S.Servers;

namespace L4D2PlayStats.Web.Models;

public class ServerModel(string serverIp, ServerInfo? serverInfo, List<Player>? players)
{
    public string ServerIp { get; } = serverIp;
    public ServerInfo? ServerInfo { get; } = serverInfo;
    public List<Player>? Players { get; } = players;
}