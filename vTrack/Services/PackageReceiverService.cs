using System.Net;
using System.Net.Sockets;
using System.Text;

namespace vTrack.Services;

public class PackageReceiverService : BackgroundService
{
    private TcpListener server;
    Int32 port = 13000;
    IPAddress ip_address = IPAddress.Parse("127.0.0.1");
    private readonly IServiceScopeFactory _scopeFactory;

    public PackageReceiverService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    private async Task Listen(TcpListener server)
    {

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

        using(var scope = _scopeFactory.CreateScope())
        {
            var packageService = scope.ServiceProvider.GetRequiredService<PackageService>();
            await packageService.StorePackageAsync(data);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        server = new TcpListener(ip_address, port);
        server.Start();

        while(!cancellationToken.IsCancellationRequested)
        {
            await Listen(server);
        }
    }
}