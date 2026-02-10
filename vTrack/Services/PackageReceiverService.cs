using System.Net;
using System.Net.Sockets;
using System.Text;

namespace vTrack.Services;

public class PackageReceiverService : BackgroundService
{
    TcpListener server;
    PackageService _packageService;

    public PackageReceiverService(PackageService packageService)
    {
        _packageService = packageService;
    }
    private async Task Listen()
    {
        Int32 port = 13000;
        IPAddress ip_address = IPAddress.Parse("127.0.0.1");

        server = new TcpListener(ip_address, port);

        server.Start();

        Byte[] bytes = new Byte[2048];
        String data = null;
        StringBuilder sb = new StringBuilder();

        using  TcpClient client = await server.AcceptTcpClientAsync();

        NetworkStream stream = client.GetStream();

        int i;

        while((i =  await stream.ReadAsync(bytes, 0, bytes.Length)) != 0)
        {
            sb.Append(Convert.ToHexString(bytes[0..i]));
        }

        data = sb.ToString();

        await _packageService.StorePackageAsync(data);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while(!cancellationToken.IsCancellationRequested)
        {
            await Listen();
        }
    }
}