namespace L4D2PlayStats.Core.Infrastructure.Options;

public interface IAppOptionsWraper
{
    string ServerId { get; }
    string SteamApiKey { get; }
    string[] ServerIps { get; }
    string PrimaryServerIp { get; }
    string[] ServerAdmins { get; }
}