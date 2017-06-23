using System;
using System.Collections.Generic;
using HotFix.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HotFix.Specification.logon
{
    [TestClass]
    public class logon
    {
        [TestMethod]
        public void successful()
        {
            var instructions = new Queue<string>(new List<string>
            {
                "! 20170623-14:51:45.012",
                // The engine should sent a valid logon message first
                "> 8=FIX.4.29=0007235=A34=152=20170623-14:51:45.01249=Client56=Server98=0108=5141=Y10=094",
                "! 20170623-14:51:45.051",
                // The engine should accept the targets logon response
                "< 8=FIX.4.29=7235=A34=149=Server52=20170623-14:51:45.05156=Client98=0108=5141=Y10=209",
                "! 20170623-14:51:46.000"
            });

            var configuration = new Configuration
            {
                Version = "FIX.4.2",
                Sender = "Client",
                Target = "Server",
                HeartbeatInterval = 5,
                OutboundSeqNum = 1,
                InboundSeqNum = 1
            };

            var engine = new Engine(configuration) { Transports = c => new Harness(instructions) };

            try
            {
                engine.Run();
            }
            catch (DelightfullySuccessfulException e)
            {
                Console.WriteLine("Test succeeded");
                return;
            }

            throw new Exception("Poop");
        }
    }
}