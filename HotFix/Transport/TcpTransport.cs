using System;
using System.Net;
using System.Net.Sockets;
using HotFix.Core;

namespace HotFix.Transport
{
    public class TcpTransport : ITransport
    {
        private readonly NetworkStream _stream;

        public TcpTransport(string address, int port)
        {
            var endpoint = new DnsEndPoint(address, port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = 1000
            };

            socket.Connect(endpoint);

            _stream = new NetworkStream(socket, true);
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
