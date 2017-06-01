using System;

namespace HotFix.Core
{
    public struct Field
    {
        private readonly string _message;
        private readonly Segment _segment;

        public int Tag { get; }
        public int Int => _message.GetInt(_segment.Offset, _segment.Length);
        public double Float => _message.GetFloat(_segment.Offset, _segment.Length);
        public string String => _message.Substring(_segment.Offset, _segment.Length);
        public DateTime DateTime => _message.GetDateTime(_segment.Offset, _segment.Length);

        public Field(string message, int tag, Segment segment)
        {
            Tag = tag;

            _message = message;
            _segment = segment;
        }
    }
}