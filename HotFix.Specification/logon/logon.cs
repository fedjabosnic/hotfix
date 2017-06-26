using System;
using System.Collections.Generic;
using FluentAssertions;
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
            var instructions = new List<string>
            {
                "! 20170623-14:51:45.012",
                // The engine should sent a valid logon message first
                "> 8=FIX.4.29=0007235=A34=152=20170623-14:51:45.01249=Client56=Server98=0108=5141=Y10=094",
                "! 20170623-14:51:45.051",
                // The engine should accept the target's logon response
                "< 8=FIX.4.29=7235=A34=149=Server52=20170623-14:51:45.05156=Client98=0108=5141=Y10=209",
                "! 20170623-14:51:46.000"
            };

            var configuration = new Configuration
            {
                Version = "FIX.4.2",
                Sender = "Client",
                Target = "Server",
                HeartbeatInterval = 5,
                OutboundSeqNum = 1,
                InboundSeqNum = 1
            };

            var harness = (Harness)null;
            var engine = new Engine(configuration) { Transports = c => harness = new Harness(instructions) };

            try
            {
                engine.Run();
            }
            catch (DelightfullySuccessfulException)
            {
                Console.WriteLine("Test succeeded");
            }
            finally
            {
                Console.WriteLine();

                for (var i = 0; i < instructions.Count; i++)
                {
                    Console.WriteLine($"{i + 1}: {instructions[i]}");

                    if (i < harness.Step)
                        Console.WriteLine("   - SUCCEEDED");
                    if (i == harness.Step)
                        Console.WriteLine("   - FAILED");
                    if (i > harness.Step)
                        Console.WriteLine("   - SKIPPED");
                }
            }

            // Verify that the messages have been consumed
            engine.Configuration.InboundSeqNum.Should().Be(2);
            engine.Configuration.OutboundSeqNum.Should().Be(2);
        }
    }
}