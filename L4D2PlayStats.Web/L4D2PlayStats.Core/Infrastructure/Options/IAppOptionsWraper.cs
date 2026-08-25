namespace L4D2PlayStats.Core.Infrastructure.Options;

public interface IAppOptionsWraper
{
    string ServerId { get; }
    string SteamApiKey { get; }
    string ServerDns { get; }
    string ServerIp { get; }
    string[] ServerAdmins { get; }
}