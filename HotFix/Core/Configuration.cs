using System;
using System.Collections.Generic;

namespace HotFix.Core
{
    public class Configuration
    {
        public Role Role { get; set; }

        public string Version { get; set; }
        public string Target { get; set; }
        public string Sender { get; set; }

        public string Host { get; set; }
        public int Port { get; set; }

        public int HeartbeatInterval { get; set; }
        public long InboundSeqNum { get; set; }
        public long OutboundSeqNum { get; set; }

        public string LogFile { get; set; }

        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
