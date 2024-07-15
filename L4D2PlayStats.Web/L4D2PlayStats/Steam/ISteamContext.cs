using L4D2PlayStats.Steam.SteamUser.Services;

namespace L4D2PlayStats.Steam;

public interface ISteamContext
{
    ISteamUserService SteamUserService { get; }
}