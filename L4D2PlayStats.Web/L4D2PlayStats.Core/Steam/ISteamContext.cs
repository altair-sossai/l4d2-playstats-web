using L4D2PlayStats.Core.Steam.ServerInfo.Services;
using L4D2PlayStats.Core.Steam.SteamUser.Services;

namespace L4D2PlayStats.Core.Steam;

public interface ISteamContext
{
    ISteamUserService SteamUserService { get; }
    IServerInfoService ServerInfoService { get; }
}