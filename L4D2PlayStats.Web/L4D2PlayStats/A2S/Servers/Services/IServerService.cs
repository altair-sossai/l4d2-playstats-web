namespace L4D2PlayStats.A2S.Servers.Services;

public interface IServerService
{
    Task<ServerInfo> GetServerInfoAsync(string ip, int port);
}