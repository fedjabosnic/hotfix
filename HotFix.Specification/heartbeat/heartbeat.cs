using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification.heartbeat
{
    [TestClass]
    public class heartbeat
    {
        [TestMethod]
        public void is_sent_when_no_outbound_message_has_been_sent_for_longer_than_the_heartbeat_interval()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Initiator,
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
                    // The engine should sent a valid logon message first
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:45.012|49=Client|56=Server|108=5|98=0|141=Y|10=094|",
                    "! 20170623-14:51:45.051",
                    // The engine should accept the target's logon response
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=209|",
                    "! 20170623-14:51:46.000",
                    "! 20170623-14:51:47.000",
                    "! 20170623-14:51:48.000",
                    "! 20170623-14:51:49.000",
                    "! 20170623-14:51:50.100",
                    // The engine should send the first heartbeat only after 5 seconds have elapsed
                    "> 8=FIX.4.2|9=00055|35=0|34=2|52=20170623-14:51:50.100|49=Client|56=Server|10=049|",
                    "! 20170623-14:51:50.056",
                    // The engine should accept the target's heartbeat
                    "< 8=FIX.4.2|9=55|35=0|34=2|49=Server|52=20170623-14:51:50.056|56=Client|10=171|",
                    "! 20170623-14:51:51.000"
                })
                .Verify((engine, configuration) =>
                {
                    engine.State.InboundSeqNum.Should().Be(3);
                    engine.State.OutboundSeqNum.Should().Be(3);
                })
                .Run();
        }
    }
}