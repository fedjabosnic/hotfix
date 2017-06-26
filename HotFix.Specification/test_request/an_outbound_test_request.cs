using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification.test_request
{
    [TestClass]
    public class an_outbound_test_request
    {
        [TestMethod]
        public void is_sent_when_no_messages_have_been_received_for_slightly_longer_than_the_heartbeat_interval()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Version = "FIX.4.2",
                    Sender = "Client",
                    Target = "Server",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:45.012",
                    "> 8=FIX.4.29=0007235=A34=152=20170623-14:51:45.01249=Client56=Server108=598=0141=Y10=094",
                    "! 20170623-14:51:45.051",
                    "< 8=FIX.4.29=7235=A34=149=Server52=20170623-14:51:45.05156=Client108=598=0141=Y10=209",
                    "! 20170623-14:51:46.000",
                    "! 20170623-14:51:47.000",
                    "! 20170623-14:51:48.000",
                    "! 20170623-14:51:49.000",
                    "! 20170623-14:51:50.100",
                    "> 8=FIX.4.29=0005535=034=252=20170623-14:51:50.10049=Client56=Server10=049",
                    "! 20170623-14:51:50.056",
                    "! 20170623-14:51:51.000",
                    "! 20170623-14:51:52.000",
                    // The engine should send a test request since no inbound messages have been received
                    "> 8=FIX.4.29=0007835=134=352=20170623-14:51:52.00049=Client56=Server112=63633826312000000010=150",
                })
                .Verify((engine, configuration) =>
                {
                    configuration.InboundSeqNum.Should().Be(2);
                    configuration.OutboundSeqNum.Should().Be(4);
                })
                .Run();
        }
    }
}