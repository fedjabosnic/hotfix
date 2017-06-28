using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification.resend_request
{
    [TestClass]
    public class an_inbound_resend_request
    {
        [TestMethod]
        public void is_rejected_when_the_end_seq_num_is_not_zero()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Version = "FIX.4.2",
                    Sender = "Client",
                    Target = "Server",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 10,
                    InboundSeqNum = 10
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:45.012",
                    "> 8=FIX.4.2|9=00073|35=A|34=10|52=20170623-14:51:45.012|49=Client|56=Server|108=5|98=0|141=Y|10=143|",
                    "! 20170623-14:51:45.051",
                    "< 8=FIX.4.2|9=73|35=A|34=10|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=002|",
                    "! 20170623-14:51:46.000",
                    "! 20170623-14:51:47.000",
                    "< 8=FIX.4.2|9=65|35=2|34=11|49=Server|52=20170623-14:51:47.000|56=Client|7=5|16=8|10=096|",
                    "! 20170623-14:51:51.000"
                })
                .Expect<EngineException>("Unsupported resend request received (partial gap fills are not supported)")
                .Run();
        }

        [TestMethod]
        public void is_responded_to_with_a_gap_fill()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Version = "FIX.4.2",
                    Sender = "Client",
                    Target = "Server",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 10,
                    InboundSeqNum = 10
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:45.012",
                    "> 8=FIX.4.2|9=00073|35=A|34=10|52=20170623-14:51:45.012|49=Client|56=Server|108=5|98=0|141=Y|10=143|",
                    "! 20170623-14:51:45.051",
                    "< 8=FIX.4.2|9=73|35=A|34=10|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=002|",
                    "! 20170623-14:51:46.000",
                    "! 20170623-14:51:47.000",
                    "< 8=FIX.4.2|9=65|35=2|34=11|49=Server|52=20170623-14:51:47.000|56=Client|7=5|16=0|10=088|",
                    "> 8=FIX.4.2|9=00067|35=4|34=5|52=20170623-14:51:47.000|49=Client|56=Server|123=Y|36=11|10=118|",
                    "! 20170623-14:51:51.000"
                })
                .Verify((engine, configuration) =>
                {
                    configuration.InboundSeqNum.Should().Be(12L);
                    configuration.OutboundSeqNum.Should().Be(11L);
                })
                .Run();
        }
    }
}