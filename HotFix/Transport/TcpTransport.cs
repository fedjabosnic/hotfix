using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using HotFix.Utilities;

namespace HotFix.Transport
{
    public class TcpTransport : ITransport
    {
        private readonly IClock _clock;
        private readonly Socket _socket;
        private readonly TimeSpan _interval;
        private readonly TimeSpan _afterburn;
        private DateTime _last;

        public static TcpTransport Create(IClock clock, bool server, string address, int port)
        {
            return server ? Accept(clock, address, port) : Initiate(clock, address, port);
        }

        public static TcpTransport Initiate(IClock clock, string address, int port)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(address), port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 1000, NoDelay = true };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) socket.EnableFastPath();
            socket.Connect(endpoint);

            return new TcpTransport(clock, socket);
        }

        public static TcpTransport Accept(IClock clock, string address, int port)
        {
            var endpoint = new IPEndPoint(IPAddress.Parse(address), port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) { ReceiveTimeout = 1000, NoDelay = true };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) socket.EnableFastPath();
            socket.Bind(endpoint);
            socket.Listen(1);

            var client = socket.Accept();

            socket.Dispose();

            return new TcpTransport(clock, client);
        }

        public TcpTransport(IClock clock, Socket socket)
        {
            _clock = clock;
            _socket = socket;
            _interval = TimeSpan.Parse("00:00:01");
            _afterburn = TimeSpan.Parse("00:00:01");
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var last = _last.Ticks;
            var start = _clock.Time.Ticks;
            var interval = start + _interval.Ticks;
            var afterburn = last + _afterburn.Ticks;

            afterburn = interval < afterburn ? interval : afterburn;

            // Afterburn mode
            while (_clock.Time.Ticks < afterburn)
            {
                // Check for data without blocking to avoid context switching
                if (_socket.Poll(0)) goto read;
            }

            var now = _clock.Time.Ticks;
            var remaining = (int)(interval - now);

            // Normal mode
            if (remaining > 0)
            {
                // Block for data up to the remaining time (possible context switch if data isn't immediately available)
                if (_socket.Poll(remaining / 10)) goto read;
            }

            return 0;

            read:

            _last = _clock.Time;

            return _socket.Receive(buffer, offset, count, SocketFlags.None);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _socket.Send(buffer, offset, count, SocketFlags.None);
        }

        public void Dispose()
        {
            _socket?.Dispose();
        }
    }
}
