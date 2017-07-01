using System;

namespace HotFix.Core
{
    public class State
    {
        public long InboundSeqNum;
        public long OutboundSeqNum;

        public DateTime InboundTimestamp;
        public DateTime OutboundTimestamp;

        public bool Synchronizing;
        public bool TestRequestPending;
    }
}