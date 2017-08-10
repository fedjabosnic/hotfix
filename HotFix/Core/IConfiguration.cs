using System;
using System.Collections.Generic;

namespace HotFix.Core
{
    public interface IConfiguration
    {
        Role Role { get; set; }

        string Version { get; set; }
        string Target { get; set; }
        string Sender { get; set; }

        string Host { get; set; }
        int Port { get; set; }

        int HeartbeatInterval { get; set; }
        long InboundSeqNum { get; set; }
        long OutboundSeqNum { get; set; }

        string LogFile { get; set; }

        List<ISchedule> Sessions { get; set; }
    }
}