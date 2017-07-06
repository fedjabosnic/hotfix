using System;
using HotFix.Transport;

namespace HotFix.Core
{
    /// <summary>
    /// A channel is a message layer abstraction over a transport, allowing messages
    /// to be written and read from a transport.
    /// </summary>
    public class Channel
    {
        public Action<byte[], int, int> Inbound { get; set; }
        public Action<byte[], int, int> Outbound { get; set; }

        private const char SOH = '\u0001';

        private readonly byte[] _buffer;
        private int _tail;
        private int _head;
        private int _current;

        public ITransport Transport { get; }

        /// <summary>
        /// Creates a new channel with the specified buffer for the provided transport.
        /// </summary>
        /// <param name="transport">The underlying transport.</param>
        /// <param name="bufferSize">The buffer size.</param>
        public Channel(ITransport transport, int bufferSize)
        {
            Transport = transport;

            _buffer = new byte[bufferSize];
        }

        /// <summary>
        /// Reads a message from the transport and returns true/false to indicate whether a valid message was read.
        /// </summary>
        /// <param name="message">The message to read into.</param>
        /// <returns>Whether a valid message was read.</returns>
        public bool Read(FIXMessage message)
        {
            if (_current == _head)
            {
                _head += Transport.Read(_buffer, _head, _buffer.Length - _head);
            }

            for (; _current < _head; _current++)
            {
                if (_current - _tail < 8) continue;

                if (_buffer[_current - 0] != SOH) continue;
                if (_buffer[_current - 4] != '=') continue;
                if (_buffer[_current - 5] != '0') continue;
                if (_buffer[_current - 6] != '1') continue;
                if (_buffer[_current - 7] != SOH) continue;

                message.Parse(_buffer, _tail, _current - _tail + 1);

                Inbound?.Invoke(_buffer, _tail, _current - _tail + 1);

                _tail = _current + 1;

                // If there's no more buffered data - reset the buffer
                if (_tail == _head) _tail = _head = _current = 0;

                return message.Valid;
            }

            // If there's no more buffer space - reset the buffer
            if (_head == _buffer.Length && _head == _current)
            {
                var buffered = _head - _tail;

                // NOTE: This may or may not be needed but we put it in for safety
                if(_tail < buffered) throw new Exception("Unable to reset buffer - not enough space");

                Buffer.BlockCopy(_buffer, _tail, _buffer, 0, buffered);

                _tail = 0;
                _head = _current = buffered;
            }

            return false;
        }

        /// <summary>
        /// Writes a message to the transport.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public void Write(FIXMessageWriter message)
        {
            Transport.Write(message.Buffer, 0, message.Length);
            Outbound?.Invoke(message.Buffer, 0, message.Length);
        }
    }
}
