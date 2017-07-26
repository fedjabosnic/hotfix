using System;

namespace HotFix.Core
{
    /// <summary>
    /// Holds runtime session state
    /// <remarks>
    /// For performance reasons, this class purposefully exposes fields rather than properties
    /// </remarks>
    /// </summary>
    public class State
    {
        public long InboundSeqNum;
        public long OutboundSeqNum;

        public DateTime InboundTimestamp;
        public DateTime OutboundTimestamp;

        public TimeSpan HeartbeatInterval;
        public TimeSpan HeartbeatTimeoutMin;
        public TimeSpan HeartbeatTimeoutMax;

        public bool Synchronizing;
        public bool TestRequestPending;
    }
}