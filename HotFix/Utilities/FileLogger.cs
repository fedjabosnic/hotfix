using System;
using System.IO;
using HotFix.Encoding;

namespace HotFix.Utilities
{
    /// <summary>
    /// A file logger.
    /// <remarks>
    /// Thread safe and garbage free.
    /// </remarks>
    /// </summary>
    public class FileLogger : ILogger
    {
        private readonly IClock _clock;
        private readonly FileStream _file;
        private readonly byte[] _inbound = new byte[26];
        private readonly byte[] _outbound = new byte[26];

        /// <summary>
        /// Creates a new instance of the file logger.
        /// </summary>
        /// <param name="clock">A clock to use for log entries.</param>
        /// <param name="file">The file to log to.</param>
        public FileLogger(IClock clock, string file)
        {
            _clock = clock;
            _file = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan);

            _inbound[0] = (byte)'\r';
            _inbound[1] = (byte)'\n';
            // Reserved space for timestamp
            _inbound[23] = (byte)' ';
            _inbound[24] = (byte)'<';
            _inbound[25] = (byte)' ';

            _outbound[0] = (byte)'\r';
            _outbound[1] = (byte)'\n';
            // Reserved space for timestamp
            _outbound[23] = (byte)' ';
            _outbound[24] = (byte)'>';
            _outbound[25] = (byte)' ';
        }

        /// <summary>
        /// Logs an inbound message within the specified boundaries in the provided buffer to the underlying log file.
        /// <remarks>
        /// The underlying log file will be soft flushed once the log line is written.
        /// </remarks>
        /// </summary>
        /// <param name="buffer">The buffer containing the message.</param>
        /// <param name="offset">The offset where the message starts.</param>
        /// <param name="length">The length of the message.</param>
        public void Inbound(byte[] buffer, int offset, int length)
        {
            if (!_file.CanWrite) throw new Exception("The log file cannot be written to");

            lock (_file)
            {
                _inbound.WriteDateTime(2, _clock.Time);

                _file.Write(_inbound, 0, 26);
                _file.Write(buffer, offset, length);

                _file.Flush();
            }
        }

        /// <summary>
        /// Logs an outbound message within the specified boundaries in the provided buffer to the underlying log file.
        /// <remarks>
        /// The underlying log file will be soft flushed once the log line is written.
        /// </remarks>
        /// </summary>
        /// <param name="buffer">The buffer containing the message.</param>
        /// <param name="offset">The offset where the message starts.</param>
        /// <param name="length">The length of the message.</param>
        public void Outbound(byte[] buffer, int offset, int length)
        {
            if (!_file.CanWrite) throw new Exception("The log file cannot be written to");

            lock (_file)
            {
                _outbound.WriteDateTime(2, _clock.Time);

                _file.Write(_outbound, 0, 26);
                _file.Write(buffer, offset, length);

                _file.Flush();
            }
        }

        /// <summary>
        /// Disposes the logger and the underlying file.
        /// <remarks>
        /// The underlying file will be hard flushed to disk first.
        /// </remarks>
        /// </summary>
        public void Dispose()
        {
            _file.Flush(true);
            _file?.Dispose();
        }
    }
}