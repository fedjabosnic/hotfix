using System;

namespace HotFix.Transport
{
    public interface ITransport : IDisposable
    {
        int Read(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
    }
}