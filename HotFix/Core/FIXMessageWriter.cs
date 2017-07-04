using System;
using HotFix.Encoding;

namespace HotFix.Core
{
    public class FIXMessageWriter
    {
        private const byte SOH = (byte)'\u0001';
        private const byte EQL = (byte) '=';

        internal readonly byte[] _buffer;
        internal int _end;

        private readonly int _headerEnd;
        private int _bodyEnd;
        private int _trailerEnd;


        public FIXMessageWriter(int maxLength, string beginString)
        {
            _buffer = new byte[maxLength];

            _end += _buffer.WriteString(_end, "8=");
            _end += _buffer.WriteString(_end, beginString);
            _buffer[_end++] = SOH;

            _end += _buffer.WriteString(_end, "9=");
            _end += _buffer.WriteString(_end, "00000");
            _buffer[_end] = SOH;

            _headerEnd = _end;
            _end++;

            _end += _buffer.WriteString(_end, "35=");
        }

        public FIXMessageWriter Prepare(string messageType)
        {
            _end = _headerEnd + 4;

            _end += _buffer.WriteString(_end, messageType);
            _buffer[_end++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, string value)
        {
            _end += _buffer.WriteInt(_end, tag);
            _buffer[_end++] = EQL;

            _end += _buffer.WriteString(_end, value);
            _buffer[_end++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, int value)
        {
            _end += _buffer.WriteInt(_end, tag);
            _buffer[_end++] = EQL;

            _end += _buffer.WriteInt(_end, value);
            _buffer[_end++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, DateTime value)
        {
            _end += _buffer.WriteInt(_end, tag);
            _buffer[_end++] = EQL;

            _end += _buffer.WriteDateTime(_end, value);
            _buffer[_end++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, long value)
        {
            _end += _buffer.WriteInt(_end, tag);
            _buffer[_end++] = EQL;

            _end += _buffer.WriteLong(_end, value);
            _buffer[_end++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, double value)
        {
            _end += _buffer.WriteInt(_end, tag);
            _buffer[_end++] = EQL;

            _end += _buffer.WriteFloat(_end, value);
            _buffer[_end++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, FIXField field)
        {
            _end += _buffer.WriteInt(_end, field.Tag);
            _buffer[_end++] = EQL;

            Buffer.BlockCopy(field._message, field._value.Offset, _buffer, _end, field._value.Length);
            _end += field._value.Length;
            _buffer[_end++] = SOH;

            return this;
        }

        public FIXMessageWriter Build()
        {
            _bodyEnd = _end - 1;

            var length = _bodyEnd - _headerEnd;
            _buffer.WriteString(_headerEnd - 5, "00000");
            _buffer.WriteIntBackwards(_headerEnd - 1, length);

            var checksum = CalculateChecksum();
            _end += _buffer.WriteString(_end, "10=000\u0001");

            _trailerEnd = _end;

            _buffer.WriteIntBackwards(_trailerEnd - 2, checksum);

            return this;
        }

        private int CalculateChecksum()
        {
            var checksum = 0;

            for (var i = 0; i <= _bodyEnd; i++)
            {
                checksum += _buffer[i];
            }

            return checksum % 256;
        }

        public override string ToString() => System.Text.Encoding.ASCII.GetString(_buffer, 0, _end);
    }
}
