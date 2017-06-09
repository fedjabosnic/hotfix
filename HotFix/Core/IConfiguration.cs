using System;
using HotFix.Transport;

namespace HotFix.Core
{
    public interface IConfiguration
    {
        int HeartbeatInterval { get; set; }

        string Version { get; set; }

        string Target { get; set; }
        string Sender { get; set; }

        string Host { get; set; }
        int Port { get; set; }

        bool Synchronised { get; set; }

        long InboundSeqNum { get; set; }
        long OutboundSeqNum { get; set; }

        int InboundBufferSize { get; set; }
        int OutboundBufferSize { get; set; }

        DateTime InboundUpdatedAt { get; set; }
        DateTime OutboundUpdatedAt { get; set; }
        
    }
}