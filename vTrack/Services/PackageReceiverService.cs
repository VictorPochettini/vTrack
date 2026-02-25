using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

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
    private static int IndexOfSequence(List<byte> buffer, byte[] seq)
    {
        if (seq.Length == 0) return -1;
        for (int i = 0; i <= buffer.Count - seq.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < seq.Length; j++)
            {
                if (buffer[i + j] != seq[j])
                {
                    match = false;
                    break;
                }
            }
            if (match) return i;
        }
        return -1;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        server = new TcpListener(ip_address, port);
        server.Start();

        while (!cancellationToken.IsCancellationRequested)
        {
            using TcpClient client = await server.AcceptTcpClientAsync();
            using NetworkStream stream = client.GetStream();

            var buffer = new List<byte>();
            byte[] readBuffer = new byte[2048];
            byte[] terminator = new byte[] { 0x0D, 0x0A };

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    int bytesRead = await stream.ReadAsync(readBuffer, 0, readBuffer.Length, cancellationToken);
                    if (bytesRead == 0) break;

                    // Append received bytes
                    for (int i = 0; i < bytesRead; i++) buffer.Add(readBuffer[i]);

                    int termIndex;
                    while ((termIndex = IndexOfSequence(buffer, terminator)) != -1)
                    {
                        int messageLength = termIndex + terminator.Length;
                        byte[] messageBytes = new byte[messageLength];
                        buffer.CopyTo(0, messageBytes, 0, messageLength);

                        string messageHex = Convert.ToHexString(messageBytes);

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var packageService = scope.ServiceProvider.GetRequiredService<PackageService>();
                            await packageService.StorePackageAsync(messageHex);
                        }

                        buffer.RemoveRange(0, messageLength);
                    }
                }
            }
            catch (OperationCanceledException) { break; }
            catch (Exception)
            {
                // swallowing exception for now; consider logging
            }
        }
    }
}