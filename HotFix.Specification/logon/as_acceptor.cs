using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification.logon
{
    [TestClass]
    public class as_acceptor
    {
        [TestMethod]
        public void fails_when_the_logon_request_is_not_received_in_the_allotted_time()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    "! 20170623-14:51:42.000",
                    "! 20170623-14:51:44.000",
                    "! 20170623-14:51:46.000",
                    "! 20170623-14:51:48.000",
                    // No logon request received from the target for over 10 seconds
                    "! 20170623-14:51:50.000"
                })
                .Expect<EngineException>("Logon request not received on time")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_logon_request_is_not_a_logon_message()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The engine should fail because the initiator sends an unexpected message type (35)
                    "< 8=FIX.4.2|9=55|35=0|34=1|52=20170623-14:51:40.000|49=Client|56=Server|10=158|"
                })
                .Expect<EngineException>("Unexpected first message received (expected a logon)")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_logon_request_contains_an_unexpected_beginstring()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The engine should fail because the initiator sends an unexpected begin string (8)
                    "< 8=FIX.4.4|9=72|35=A|34=1|52=20170623-14:51:40.000|49=Client|56=Server|108=5|98=0|141=Y|10=200|"
                })
                .Expect<EngineException>("Unexpected begin string received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_logon_request_contains_an_unexpected_sender_compid()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The engine should fail because the initiator sends an unexpected sender comp id (49)
                    "< 8=FIX.4.2|9=72|35=A|34=1|52=20170623-14:51:40.000|49=XXXXXX|56=Server|108=5|98=0|141=Y|10=119|"
                })
                .Expect<EngineException>("Unexpected comp id received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_logon_request_contains_an_unexpected_target_compid()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The engine should fail because the initiator sends an unexpected target comp id (56)
                    "< 8=FIX.4.2|9=72|35=A|34=1|52=20170623-14:51:40.000|49=Client|56=XXXXXX|108=5|98=0|141=Y|10=095|"
                })
                .Expect<EngineException>("Unexpected comp id received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_logon_request_contains_an_unexpected_heartbeat_interval()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The engine should fail because the initiator sends an unexpected heartbeat interval (108)
                    "< 8=FIX.4.2|9=72|35=A|34=1|52=20170623-14:51:40.000|49=Client|56=Server|108=8|98=0|141=Y|10=201|"
                })
                .Expect<EngineException>("Unexpected heartbeat interval received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_logon_request_contains_an_unexpected_encryption_method()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The engine should fail because the initiator sends an unexpected excryption method (98)
                    "< 8=FIX.4.2|9=72|35=A|34=1|52=20170623-14:51:40.000|49=Client|56=Server|108=5|98=3|141=Y|10=201|"
                })
                .Expect<EngineException>("Unexpected encryption method received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_logon_request_contains_an_unexpected_reset_on_logon()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The engine should fail because the initiator sends an unexpected reset on logon (141)
                    "< 8=FIX.4.2|9=72|35=A|34=1|52=20170623-14:51:40.000|49=Client|56=Server|108=5|98=0|141=N|10=187|"
                })
                .Expect<EngineException>("Unexpected reset on logon received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_logon_request_contains_a_sequence_number_that_is_too_low()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 5
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The engine should fail because the initiator sends a sequence number (34) that is too low
                    "< 8=FIX.4.2|9=72|35=A|34=1|52=20170623-14:51:40.000|49=Client|56=Server|108=5|98=0|141=Y|10=198|"
                })
                .Expect<EngineException>("Sequence number too low")
                .Run();
        }

        [TestMethod]
        public void fails_when_a_logon_request_is_received_while_already_successfully_logged_on()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The initiator sends a logon request with a sequence number (34) that is as expected
                    "< 8=FIX.4.2|9=72|35=A|34=1|52=20170623-14:51:40.000|49=Client|56=Server|108=5|98=0|141=Y|10=198|",
                    // The engine sends a logon response
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:40.000|49=Server|56=Client|108=5|98=0|141=Y|10=086|",
                    // The initiator sends another logon request after the logon has already succeeded
                    "< 8=FIX.4.2|9=72|35=A|34=2|52=20170623-14:51:40.000|49=Client|56=Server|108=5|98=0|141=Y|10=199|"
                })
                .Expect<EngineException>("Logon message received while already logged on")
                .Run();
        }

        [TestMethod]
        public void succeeds_but_sends_a_resend_request_when_the_logon_request_contains_a_sequence_number_that_is_too_high()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The initiator sends a logon request with a sequence number (34) that is too high
                    "< 8=FIX.4.2|9=72|35=A|34=5|52=20170623-14:51:40.000|49=Client|56=Server|108=5|98=0|141=Y|10=202|",
                    // The engine sends a logon response
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:40.000|49=Server|56=Client|108=5|98=0|141=Y|10=086|",
                    // The engine sends a resend request for any missed messages (sequence number 1 onwards)
                    "> 8=FIX.4.2|9=00064|35=2|34=2|52=20170623-14:51:40.000|49=Server|56=Client|7=1|16=0|10=172|"
                })
                .Verify((engine, configuration) =>
                {
                    engine.State.InboundSeqNum.Should().Be(1);
                    engine.State.OutboundSeqNum.Should().Be(3);
                    engine.State.Synchronizing.Should().Be(true);
                })
                .Run();
        }

        [TestMethod]
        public void succeeds_when_a_valid_logon_request_is_received_promptly_with_the_expected_sequence_numbers()
        {
            new Specification()
                .Configure(new Configuration
                {
                    Role = Role.Acceptor,
                    Version = "FIX.4.2",
                    Sender = "Server",
                    Target = "Client",
                    HeartbeatInterval = 5,
                    OutboundSeqNum = 1,
                    InboundSeqNum = 1
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:40.000",
                    // The initiator sends a logon request with a sequence number (34) that is as expected
                    "< 8=FIX.4.2|9=72|35=A|34=1|52=20170623-14:51:40.000|49=Client|56=Server|108=5|98=0|141=Y|10=198|",
                    // The engine sends a logon response
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:40.000|49=Server|56=Client|108=5|98=0|141=Y|10=086|",
                    "! 20170623-14:51:42.000"
                })
                .Verify((engine, configuration) =>
                {
                    engine.State.InboundSeqNum.Should().Be(2);
                    engine.State.OutboundSeqNum.Should().Be(2);
                })
                .Run();
        }
    }
}