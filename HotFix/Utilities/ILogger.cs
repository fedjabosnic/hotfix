using System;

namespace HotFix.Utilities
{
    /// <summary>
    /// An abstraction for message logging.
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Logs an inbound message within the specified boundaries in the provided buffer.
        /// </summary>
        /// <param name="buffer">The buffer containing the message.</param>
        /// <param name="offset">The offset where the message starts.</param>
        /// <param name="length">The length of the message.</param>
        void Inbound(byte[] buffer, int offset, int length);

        /// <summary>
        /// Logs an outbound message within the specified boundaries in the provided buffer.
        /// </summary>
        /// <param name="buffer">The buffer containing the message.</param>
        /// <param name="offset">The offset where the message starts.</param>
        /// <param name="length">The length of the message.</param>
        void Outbound(byte[] buffer, int offset, int length);
    }
}
