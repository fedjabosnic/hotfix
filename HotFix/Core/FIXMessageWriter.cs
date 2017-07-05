using System;
using HotFix.Encoding;

namespace HotFix.Core
{
    public class FIXMessageWriter
    {
        private const byte SOH = (byte)'\u0001';
        private const byte EQL = (byte) '=';

        private readonly byte[] _body;
        private int _bodyEnd;

        internal readonly byte[] _buffer;
        internal int _end;

        public FIXMessageWriter(int maxLength)
        {
            _buffer = new byte[maxLength];
            _body = new byte[maxLength];
        }

        public FIXMessageWriter Clear()
        {
            _end = 0;
            _bodyEnd = 0;

            return this;
        }

        public FIXMessageWriter Prepare(string beginString, string messageType, long seqNum, DateTime sendingTime, string sender, string target)
        {
            // write beginstring
            _end = _buffer.WriteString(0, "8=");
            _end += _buffer.WriteString(_end, beginString);
            _buffer[_end++] = SOH;

            // write length placeholder
            // Should probably measure how many digits are needed to represent maxLength and use that many zeros
            // The zeros string could then be cached and reused since it can be calculated at construction time
            _end += _buffer.WriteString(_end, "9=00000");
            var lengthPosition = _end - 1; // points to the last zero
            _buffer[_end++] = SOH;

            // write messagetype
            _end += _buffer.WriteString(_end, "35=");
            _end += _buffer.WriteString(_end, messageType);
            _buffer[_end++] = SOH;

            // write seqnum
            _end += _buffer.WriteString(_end, "34=");
            _end += _buffer.WriteLong(_end, seqNum);
            _buffer[_end++] = SOH;

            // write sendingTime
            _end += _buffer.WriteString(_end, "52=");
            _end += _buffer.WriteDateTime(_end, sendingTime);
            _buffer[_end++] = SOH;

            // write sender
            _end += _buffer.WriteString(_end, "49=");
            _end += _buffer.WriteString(_end, sender);
            _buffer[_end++] = SOH;

            // write target
            _end += _buffer.WriteString(_end, "56=");
            _end += _buffer.WriteString(_end, target);
            _buffer[_end++] = SOH;

            // copy body
            Buffer.BlockCopy(_body, 0, _buffer, _end, _bodyEnd);
            _end += _bodyEnd;

            // update length
            _buffer.WriteIntBackwards(lengthPosition, _end - (lengthPosition + 2));

            // calculate and write checksum
            var checksum = CalculateChecksum();
            _end += _buffer.WriteString(_end, "10=000\u0001");
            _buffer.WriteIntBackwards(_end - 2, checksum);

            return this;
        }

        public FIXMessageWriter Set(int tag, string value)
        {
            _bodyEnd += _body.WriteInt(_bodyEnd, tag);
            _body[_bodyEnd++] = EQL;

            _bodyEnd += _body.WriteString(_bodyEnd, value);
            _body[_bodyEnd++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, int value)
        {
            _bodyEnd += _body.WriteInt(_bodyEnd, tag);
            _body[_bodyEnd++] = EQL;

            _bodyEnd += _body.WriteInt(_bodyEnd, value);
            _body[_bodyEnd++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, DateTime value)
        {
            _bodyEnd += _body.WriteInt(_bodyEnd, tag);
            _body[_bodyEnd++] = EQL;

            _bodyEnd += _body.WriteDateTime(_bodyEnd, value);
            _body[_bodyEnd++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, long value)
        {
            _bodyEnd += _body.WriteInt(_bodyEnd, tag);
            _body[_bodyEnd++] = EQL;

            _bodyEnd += _body.WriteLong(_bodyEnd, value);
            _body[_bodyEnd++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, double value)
        {
            _bodyEnd += _body.WriteInt(_bodyEnd, tag);
            _body[_bodyEnd++] = EQL;

            _bodyEnd += _body.WriteFloat(_bodyEnd, value);
            _body[_bodyEnd++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, FIXField field)
        {
            _bodyEnd += _body.WriteInt(_bodyEnd, field.Tag);
            _body[_bodyEnd++] = EQL;

            Buffer.BlockCopy(field._message, field._value.Offset, _body, _bodyEnd, field._value.Length);
            _bodyEnd += field._value.Length;
            _body[_bodyEnd++] = SOH;

            return this;
        }

        private int CalculateChecksum()
        {
            var checksum = 0;

            for (var i = 0; i < _end; i++)
            {
                checksum += _buffer[i];
            }

            return checksum % 256;
        }

        public override string ToString() => System.Text.Encoding.ASCII.GetString(_buffer, 0, _end);
    }
}
