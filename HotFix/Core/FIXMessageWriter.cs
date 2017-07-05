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
        internal int End;

        public FIXMessageWriter(int maxLength)
        {
            Buffer = new byte[maxLength];
            _body = new byte[maxLength];
        }

        public FIXMessageWriter Clear()
        {
            End = 0;
            _bodyEnd = 0;

            return this;
        }

        public FIXMessageWriter Prepare(string beginString, string messageType, long seqNum, DateTime sendingTime, string sender, string target)
        {
            // write beginstring
            End = Buffer.WriteString(0, "8=");
            End += Buffer.WriteString(End, beginString);
            Buffer[End++] = SOH;

            // write length placeholder
            // Should probably measure how many digits are needed to represent maxLength and use that many zeros
            // The zeros string could then be cached and reused since it can be calculated at construction time
            End += Buffer.WriteString(End, "9=00000");
            var lengthPosition = End - 1; // points to the last zero
            Buffer[End++] = SOH;

            // write messagetype
            End += Buffer.WriteString(End, "35=");
            End += Buffer.WriteString(End, messageType);
            Buffer[End++] = SOH;

            // write seqnum
            End += Buffer.WriteString(End, "34=");
            End += Buffer.WriteLong(End, seqNum);
            Buffer[End++] = SOH;

            // write sendingTime
            End += Buffer.WriteString(End, "52=");
            End += Buffer.WriteDateTime(End, sendingTime);
            Buffer[End++] = SOH;

            // write sender
            End += Buffer.WriteString(End, "49=");
            End += Buffer.WriteString(End, sender);
            Buffer[End++] = SOH;

            // write target
            End += Buffer.WriteString(End, "56=");
            End += Buffer.WriteString(End, target);
            Buffer[End++] = SOH;

            // copy body
            System.Buffer.BlockCopy(_body, 0, Buffer, End, _bodyEnd);
            End += _bodyEnd;

            // update length
            Buffer.WriteIntBackwards(lengthPosition, End - (lengthPosition + 2));

            // calculate and write checksum
            var checksum = CalculateChecksum();
            End += Buffer.WriteString(End, "10=000\u0001");
            Buffer.WriteIntBackwards(End - 2, checksum);

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

            System.Buffer.BlockCopy(field._message, field._value.Offset, _body, _bodyEnd, field._value.Length);
            _bodyEnd += field._value.Length;
            _body[_bodyEnd++] = SOH;

            return this;
        }

        private int CalculateChecksum()
        {
            var checksum = 0;

            for (var i = 0; i < End; i++)
            {
                checksum += Buffer[i];
            }

            return checksum % 256;
        }

        public override string ToString() => System.Text.Encoding.ASCII.GetString(Buffer, 0, End);
    }
}
