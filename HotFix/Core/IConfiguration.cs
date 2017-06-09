using System;

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

        int InboundBufferSize { get; set; }
        int OutboundBufferSize { get; set; }

        bool LoggingOn { get; set; }
        bool LoggedOn { get; set; }
        bool LoggingOut { get; set; }
        bool LoggedOut { get; set; }

        bool Synchronised { get; set; }

        long InboundSeqNum { get; set; }
        long OutboundSeqNum { get; set; }

        DateTime InboundUpdatedAt { get; set; }
        DateTime OutboundUpdatedAt { get; set; }

        string InboundTestReqId { get; set; }
        string OutboundTestReqId { get; set; }
    }
}