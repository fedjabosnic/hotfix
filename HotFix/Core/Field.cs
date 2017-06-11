using System;
using HotFix.Utilities;

namespace HotFix.Core
{
    public struct Field
    {
        private readonly string _message;
        private readonly Segment _segment;

        public int Tag { get; }
        public int Length { get; }
        public int Checksum { get; }

        public int AsInt => _message.GetInt(_segment.Offset, _segment.Length);
        public long AsLong => _message.GetLong(_segment.Offset, _segment.Length);
        public double AsFloat => _message.GetFloat(_segment.Offset, _segment.Length);
        public string AsString => _message.Substring(_segment.Offset, _segment.Length);
        public DateTime AsDateTime => _message.GetDateTime(_segment.Offset, _segment.Length);

        public Field(string message, int tag, int length, int checksum, Segment segment)
        {
            Tag = tag;
            Length = length;
            Checksum = checksum;

            _message = message;
            _segment = segment;
        }
    }
}