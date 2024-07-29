using System.Net;
using System.Net.Sockets;
using System.Text;

namespace L4D2PlayStats.A2S.Servers.Services;

public class ServerService : IServerService
{
    private static readonly byte[] Request = [0xFF, 0xFF, 0xFF, 0xFF, 0x54, 0x53, 0x6F, 0x75, 0x72, 0x63, 0x65, 0x20, 0x45, 0x6E, 0x67, 0x69, 0x6E, 0x65, 0x20, 0x51, 0x75, 0x65, 0x72, 0x79, 0x00];

    public async Task<ServerInfo> GetServerInfoAsync(string ip, int port)
    {
        var ipAddress = IPAddress.Parse(ip);
        var ipEndPoint = new IPEndPoint(ipAddress, port);

        using var udpClient = new UdpClient();
        udpClient.Client.SendTimeout = 5000;
        udpClient.Client.ReceiveTimeout = 5000;

        await udpClient.SendAsync(Request, Request.Length, ipEndPoint);

        var result = await udpClient.ReceiveAsync();
        var bytes = result.Buffer;

        if (bytes.Length > 5 && bytes[4] == 0x41)
        {
            var challenge = new byte[Request.Length + 4];
            Array.Copy(Request, challenge, Request.Length);
            Array.Copy(bytes, 5, challenge, Request.Length, 4);
            await udpClient.SendAsync(challenge, challenge.Length, ipEndPoint);

            result = await udpClient.ReceiveAsync();
            bytes = result.Buffer;
        }

        using var memoryStream = new MemoryStream(bytes);
        var binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);
        memoryStream.Seek(4, SeekOrigin.Begin);

        return new ServerInfo(ref binaryReader);
    }
}