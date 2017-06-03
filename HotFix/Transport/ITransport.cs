using System;
using System.IO;

namespace HotFix.Transport
{
    public interface ITransport : IDisposable
    {
        Stream Inbound { get; } // Listen
        Stream Outbound { get; } // Send
    }
}