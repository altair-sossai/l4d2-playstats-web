namespace L4D2PlayStats.Core.Infrastructure.Networking;

public interface IDnsResolver
{
    string Resolve(string address);
}