﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification.logout
{
    [TestClass]
    public class an_initiated_logout
    {
        [TestMethod]
        public void sends_a_logout_message_and_disconnects()
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
                    // The user decides to stop the session
                    "X",
                    // The engine sends a logout message
                    "> 8=FIX.4.2|9=00055|35=5|34=2|52=20170623-14:51:46.000|49=Client|56=Server|10=058|"
                })
                .Verify((session, configuration, transport) =>
                {
                    session.State.InboundSeqNum.Should().Be(2);
                    session.State.OutboundSeqNum.Should().Be(3);
                    session.Active.Should().Be(false);
                    transport.Disposed.Should().Be(true);
                })
                .Run();
        }
    }
}
