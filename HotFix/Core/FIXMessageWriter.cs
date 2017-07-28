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

        internal readonly byte[] Buffer;
        internal int Length;

        public FIXMessageWriter(int maxLength)
        {
            Buffer = new byte[maxLength];
            _body = new byte[maxLength];
        }

        public FIXMessageWriter Clear()
        {
            Length = 0;
            _bodyEnd = 0;

            return this;
        }

        public FIXMessageWriter Prepare(string beginString, string messageType, long seqNum, DateTime sendingTime, string sender, string target)
        {
            // Write beginstring
            Length = Buffer.WriteString(0, "8=");
            Length += Buffer.WriteString(Length, beginString);
            Buffer[Length++] = SOH;

            // Write length placeholder
            // TODO: Should probably measure how many digits are needed to represent maxLength and use that many zeros
            //       The zeros string could then be cached and reused since it can be calculated at construction time
            Length += Buffer.WriteString(Length, "9=00000");
            var lengthPosition = Length - 1; // points to the last zero
            Buffer[Length++] = SOH;

            // Write messagetype
            Length += Buffer.WriteString(Length, "35=");
            Length += Buffer.WriteString(Length, messageType);
            Buffer[Length++] = SOH;

            // Write seqnum
            Length += Buffer.WriteString(Length, "34=");
            Length += Buffer.WriteLong(Length, seqNum);
            Buffer[Length++] = SOH;

            // Write sendingTime
            Length += Buffer.WriteString(Length, "52=");
            Length += Buffer.WriteDateTime(Length, sendingTime);
            Buffer[Length++] = SOH;

            // Write sender
            Length += Buffer.WriteString(Length, "49=");
            Length += Buffer.WriteString(Length, sender);
            Buffer[Length++] = SOH;

            // Write target
            Length += Buffer.WriteString(Length, "56=");
            Length += Buffer.WriteString(Length, target);
            Buffer[Length++] = SOH;

            // Copy body
            System.Buffer.BlockCopy(_body, 0, Buffer, Length, _bodyEnd);
            Length += _bodyEnd;

            // Update length
            Buffer.WriteIntBackwards(lengthPosition, Length - (lengthPosition + 2));

            // Calculate and write checksum
            var checksum = CalculateChecksum();
            Length += Buffer.WriteString(Length, "10=000\u0001");
            Buffer.WriteIntBackwards(Length - 2, checksum);

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

            _bodyEnd += _body.WriteFloat(_bodyEnd, value, 6);
            _body[_bodyEnd++] = SOH;

            return this;
        }

        public FIXMessageWriter Set(int tag, FIXField field)
        {
            _bodyEnd += _body.WriteInt(_bodyEnd, field.Tag);
            _body[_bodyEnd++] = EQL;

            System.Buffer.BlockCopy(field._message, field._value.Offset, _body, _bodyEnd, field._value.Length);
            _bodyEnd += field._value.Length;
            _body[_bodyEnd++] = SOH;

            return this;
        }

        private int CalculateChecksum()
        {
            var checksum = 0;

            for (var i = 0; i < Length; i++)
            {
                checksum += Buffer[i];
            }

            return checksum % 256;
        }

        public override string ToString()
        {
            return System.Text.Encoding.ASCII.GetString(Buffer, 0, Length);
        }

        public void WriteTo(byte[] target, int offset)
        {
            System.Buffer.BlockCopy(Buffer, 0, target, offset, Length);
        }
    }
}
