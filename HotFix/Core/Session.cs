using System;
using System.IO;

namespace HotFix.Core
{
    public class Session : IDisposable
    {
        private readonly Stream _stream;

        internal Session(Stream stream)
        {
            _stream = stream;
        }

        public void Connect()
        {
            // TODO
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}
