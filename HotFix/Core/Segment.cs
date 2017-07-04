using System;

namespace HotFix.Core
{
    public struct Segment
    {
        public int Offset { get; }
        public int Length { get; }

        public Segment(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }
    }
}