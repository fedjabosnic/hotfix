using System;
using System.Net;
using System.Net.Sockets;

namespace HotFix.Transport
{
    public class TcpTransport : ITransport
    {
        const int SIO_LOOPBACK_FAST_PATH = -1744830448;

        private readonly NetworkStream _stream;

        public static TcpTransport Create(bool server, string address, int port)
        {
            return server ? Accept(address, port) : Initiate(address, port);
        }

        public static TcpTransport Initiate(string address, int port)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(address), port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 1000, NoDelay = true };

            socket.IOControl(SIO_LOOPBACK_FAST_PATH, BitConverter.GetBytes(1), null);
            socket.Connect(endpoint);

            return new TcpTransport(new NetworkStream(socket, true));
        }

        public static TcpTransport Accept(string address, int port)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(address), port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 1000, NoDelay = true };

            socket.IOControl(SIO_LOOPBACK_FAST_PATH, BitConverter.GetBytes(1), null);
            socket.Bind(endpoint);
            socket.Listen(1);

            var client = socket.Accept();

            socket.Dispose();

            return new TcpTransport(new NetworkStream(client, true));
        }

        public TcpTransport(NetworkStream stream)
        {
            _stream = stream;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var start = DateTime.UtcNow;

            while (!_stream.DataAvailable)
            {
                if (DateTime.UtcNow - start > TimeSpan.FromSeconds(1)) return 0;
            }

            return _stream.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}
