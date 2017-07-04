using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification.logon
{
    [TestClass]
    public class as_initiator
    {
        [TestMethod]
        public void fails_when_the_response_is_not_received_in_the_allotted_time()
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
                    "! 20170623-14:51:48.000",
                    "! 20170623-14:51:51.000",
                    "! 20170623-14:51:54.000",
                    // No response from the target for over 10 seconds
                    "! 20170623-14:51:57.000"
                })
                .Expect<EngineException>("Logon response not received on time")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_response_is_not_a_logon_message()
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
                    // The engine should fail because the target sends an unexpected message type
                    "< 8=FIX.4.2|9=55|35=0|34=1|49=Server|52=20170623-14:51:45.051|56=Client|10=169|"
                })
                .Expect<EngineException>("Unexpected first message received (expected a logon)")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_response_contains_an_unexpected_beginstring()
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
                    // The engine should fail because the response contains and unexpected begin string (8)
                    "< 8=FIX.4.4|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=211|"
                })
                .Expect<EngineException>("Unexpected begin string received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_response_contains_an_unexpected_sender_compid()
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
                    // The engine should fail because the response contains an unexpected sender comp id (49)
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=XXXXXX|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=106|"
                })
                .Expect<EngineException>("Unexpected comp id received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_response_contains_an_unexpected_target_compid()
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
                    // The engine should fail because the response contains an unexpected target comp id (56)
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=XXXXXX|108=5|98=0|141=Y|10=130|"
                })
                .Expect<EngineException>("Unexpected comp id received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_response_contains_an_unexpected_heartbeat_interval()
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
                    // The engine should fail because the response contains an unexpected heartbeat interval (108)
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=8|98=0|141=Y|10=212|"
                })
                .Expect<EngineException>("Unexpected heartbeat interval received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_response_contains_an_unexpected_encryption_method()
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
                    // The engine should fail because the response contains an unexpected encryption method (98)
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=3|141=Y|10=212|"
                })
                .Expect<EngineException>("Unexpected encryption method received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_response_contains_an_unexpected_reset_on_logon()
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
                    // The engine should fail because the response contains an unexpected reset on logon (141)
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Z|10=210|"
                })
                .Expect<EngineException>("Unexpected reset on logon received")
                .Run();
        }

        [TestMethod]
        public void fails_when_the_response_contains_a_sequence_number_that_is_too_low()
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
                    InboundSeqNum = 5
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:45.012",
                    // The engine should sent a valid logon message first
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:45.012|49=Client|56=Server|108=5|98=0|141=Y|10=094|",
                    "! 20170623-14:51:45.051",
                    // The engine should fail because the response contains a sequence number (34) that is too low
                    "< 8=FIX.4.2|9=72|35=A|34=1|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=209|"
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
                    // The engine should fail since we got a logon message while already successfully logged on
                    "< 8=FIX.4.2|9=72|35=A|34=2|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=210|"
                })
                .Expect<EngineException>("Logon message received while already logged on")
                .Run();
        }

        [TestMethod]
        public void succeeds_but_sends_a_resend_request_when_the_response_contains_a_sequence_number_that_is_too_high()
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
                    InboundSeqNum = 5
                })
                .Steps(new List<string>
                {
                    "! 20170623-14:51:45.012",
                    // The engine should sent a valid logon message first
                    "> 8=FIX.4.2|9=00072|35=A|34=1|52=20170623-14:51:45.012|49=Client|56=Server|108=5|98=0|141=Y|10=094|",
                    "! 20170623-14:51:45.051",
                    // The engine receives a response which contains a sequence number (34) that is too high
                    "< 8=FIX.4.2|9=72|35=A|34=7|49=Server|52=20170623-14:51:45.051|56=Client|108=5|98=0|141=Y|10=215|",
                    // The engine should send a resend request
                    "> 8=FIX.4.2|9=00064|35=2|34=2|52=20170623-14:51:45.051|49=Client|56=Server|7=5|16=0|10=187|",
                    "! 20170623-14:51:46.000"
                })
                .Verify((session, configuration, _) =>
                {
                    session.State.InboundSeqNum.Should().Be(5);
                    session.State.OutboundSeqNum.Should().Be(3);
                    session.State.Synchronizing.Should().Be(true);
                })
                .Run();
        }

        [TestMethod]
        public void succeeds_when_a_valid_logon_response_is_received_promptly_with_the_expected_sequence_numbers()
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
                    "! 20170623-14:51:46.000"
                })
                .Verify((session, configuration, _) =>
                {
                    session.State.InboundSeqNum.Should().Be(2);
                    session.State.OutboundSeqNum.Should().Be(2);
                })
                .Run();
        }
    }
}