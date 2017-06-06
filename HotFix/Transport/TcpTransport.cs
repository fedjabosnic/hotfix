using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HotFix.Transport
{
    public class TcpTransport : ITransport
    {
        private readonly Stream _stream;

        public Stream Inbound => _stream;
        public Stream Outbound => _stream;

        public TcpTransport(string address, int port)
        {
            var endpoint = new DnsEndPoint(address, port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = 1000
            };

            socket.Connect(endpoint);

            _stream = new NetworkStream(socket, true);

            Thread.Sleep(1000);

            System.Console.WriteLine(socket.Connected);
            System.Console.WriteLine();
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}
