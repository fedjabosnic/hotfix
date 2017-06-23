using System;
using System.IO;

namespace HotFix.Transport
{
    public class ConsoleTransport : ITransport
    {
        public Stream Inbound { get; private set; }
        public Stream Outbound { get; private set; }

        public ConsoleTransport()
        {
            Inbound = Console.OpenStandardInput();
            Outbound = Console.OpenStandardOutput();
        }

        public void Dispose()
        {
            Inbound = null;
            Outbound = null;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return Inbound.Read(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            Outbound.Write(buffer, offset, count);
        }
    }
}