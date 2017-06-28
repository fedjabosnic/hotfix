using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification.test_request
{
    [TestClass]
    public class an_inbound_test_request
    {
        [TestMethod]
        public void is_responded_to_with_a_heartbeat_containing_the_test_request_id()
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
                    // The engine receives a test request
                    "< 8=FIX.4.29=6535=134=249=Server52=20170623-14:51:47.00056=Client112=XXXXX10=050",
                    // The engine should respond with a heartbeat containing the test request id (112)
                    "> 8=FIX.4.29=0006535=034=252=20170623-14:51:47.00049=Client56=Server112=XXXXX10=193",
                    "! 20170623-14:51:48.000"
                })
                .Verify((engine, configuration) =>
                {
                    configuration.InboundSeqNum.Should().Be(3);
                    configuration.OutboundSeqNum.Should().Be(3);
                })
                .Run();
        }
    }
}