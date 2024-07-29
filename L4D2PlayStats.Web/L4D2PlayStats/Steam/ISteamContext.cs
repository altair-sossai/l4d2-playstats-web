using L4D2PlayStats.Steam.ServerInfo.Services;
using L4D2PlayStats.Steam.SteamUser.Services;

namespace L4D2PlayStats.Steam;

public interface ISteamContext
{
    ISteamUserService SteamUserService { get; }
    IServerInfoService ServerInfoService { get; }
}