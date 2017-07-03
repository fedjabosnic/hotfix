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
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:45.012|49=Client|56=Server|108=5|98=0|141=Y|10=094|",
                    "! 20170623-14:51:45.051",
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=209|",
                    "! 20170623-14:51:46.000",
                    "! 20170623-14:51:47.000",
                    "! 20170623-14:51:48.000",
                    "! 20170623-14:51:49.000",
                    "! 20170623-14:51:50.100",
                    "> 8=FIX.4.2|9=00055|35=0|34=2|52=20170623-14:51:50.100|49=Client|56=Server|10=049|",
                    "! 20170623-14:51:50.056",
                    "! 20170623-14:51:51.000",
                    "! 20170623-14:51:52.000",
                    // The engine should send a test request since no inbound messages have been received
                    "> 8=FIX.4.2|9=00078|35=1|34=3|52=20170623-14:51:52.000|49=Client|56=Server|112=636338263120000000|10=150|"
                })
                .Verify((engine, configuration) =>
                {
                    engine.State.InboundSeqNum.Should().Be(2);
                    engine.State.OutboundSeqNum.Should().Be(4);
                })
                .Run();
        }

        [TestMethod]
        public void terminates_the_session_when_no_message_is_received_for_twice_the_heartbeat_interval()
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
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:45.012|49=Client|56=Server|108=5|98=0|141=Y|10=094|",
                    "! 20170623-14:51:45.051",
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=209|",
                    "! 20170623-14:51:46.000",
                    "! 20170623-14:51:47.000",
                    "! 20170623-14:51:48.000",
                    "! 20170623-14:51:49.000",
                    "! 20170623-14:51:50.100",
                    "> 8=FIX.4.2|9=00055|35=0|34=2|52=20170623-14:51:50.100|49=Client|56=Server|10=049|",
                    "! 20170623-14:51:50.056",
                    "! 20170623-14:51:51.000",
                    "! 20170623-14:51:52.000",
                    // The engine should terminate the session after not receiving any messages
                    "> 8=FIX.4.2|9=00078|35=1|34=3|52=20170623-14:51:52.000|49=Client|56=Server|112=636338263120000000|10=150|",
                    "! 20170623-14:51:53.000",
                    "! 20170623-14:51:54.000",
                    "! 20170623-14:51:55.000",
                    "! 20170623-14:51:56.000"
                })
                .Expect<EngineException>("Did not receive any messages for too long")
                .Run();
        }

        [TestMethod]
        public void is_reset_when_a_message_is_received_by_the_other_party()
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
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:45.012|49=Client|56=Server|108=5|98=0|141=Y|10=094|",
                    "! 20170623-14:51:45.051",
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=209|",
                    "! 20170623-14:51:46.000",
                    "! 20170623-14:51:47.000",
                    "! 20170623-14:51:48.000",
                    "! 20170623-14:51:49.000",
                    "! 20170623-14:51:50.100",
                    "> 8=FIX.4.2|9=00055|35=0|34=2|52=20170623-14:51:50.100|49=Client|56=Server|10=049|",
                    "! 20170623-14:51:50.056",
                    "! 20170623-14:51:51.000",
                    "! 20170623-14:51:52.000",
                    // The engine should send a test request since no inbound messages have been received
                    "> 8=FIX.4.2|9=00078|35=1|34=3|52=20170623-14:51:52.000|49=Client|56=Server|112=636338263120000000|10=150|",
                    "! 20170623-14:51:53.000",
                    // The engine receives a message from the other side and so it should not terminate the session in the next 10 seconds
                    "< 8=FIX.4.2|9=55|35=0|34=2|49=Server|52=20170623-14:51:53.000|56=Client|10=163|",
                    "! 20170623-14:51:54.000",
                    "! 20170623-14:51:56.000",
                    "! 20170623-14:51:58.000",
                    // The engine should keep sending/receiving heartbeats normally
                    "> 8=FIX.4.2|9=00055|35=0|34=4|52=20170623-14:51:58.000|49=Client|56=Server|10=058|",
                    "< 8=FIX.4.2|9=55|35=0|34=3|49=Server|52=20170623-14:51:58.000|56=Client|10=169|",
                    "! 20170623-14:52:00.000",
                    "! 20170623-14:52:03.500",
                    // The engine should keep sending/receiving heartbeats normally
                    "> 8=FIX.4.2|9=00055|35=0|34=5|52=20170623-14:52:03.500|49=Client|56=Server|10=055|",
                    "< 8=FIX.4.2|9=55|35=0|34=4|49=Server|52=20170623-14:52:03.500|56=Client|10=166|",
                    "! 20170623-14:52:05.000",
                    "! 20170623-14:52:06.000"
                })
                .Verify((engine, configuration) =>
                {
                    engine.State.InboundSeqNum.Should().Be(5);
                    engine.State.OutboundSeqNum.Should().Be(6);
                })
                .Run();
        }
    }
}