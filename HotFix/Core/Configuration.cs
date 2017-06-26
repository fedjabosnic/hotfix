using System;

namespace HotFix.Core
{
    public class Configuration : IConfiguration
    {
        public int HeartbeatInterval { get; set; }

        public string Version { get; set; } = "FIX.4.2";

        public string Target { get; set; }
        public string Sender { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }
        
        public long InboundSeqNum { get; set; }
        public long OutboundSeqNum { get; set; }

        public int InboundBufferSize { get; set; }
        public int OutboundBufferSize { get; set; }

        public DateTime InboundTimestamp { get; set; }
        public DateTime OutboundTimestamp { get; set; }

        public bool Synchronizing { get; set; }
    }
}
