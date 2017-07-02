using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotFix.Core;

namespace HotFix
{
    public class Program
    {
        public static Engine Engine;
        public static Session Acceptor;
        public static Session Initiator;

        public static bool Running = true;
        public static List<long> Timings = new List<long>(10000000);

        public static void Main(string[] args)
        {
            Console.WriteLine("Performance benchmark");
            Console.WriteLine();
            Console.WriteLine(
                "This performance benchmark will run an acceptor and an initiator on two background threads " +
                "connected over loopback via tcp transport. We measure the overall one-way path of messaging " +
                "from the acceptor to the initiator, including: \n\n" +
                "user code -> encoding -> sending -> receiving -> decoding -> user code");
            Console.WriteLine();
            Console.WriteLine("Note: You can play with the message fields to see how message size affect performance");
            Console.WriteLine();
            Console.WriteLine("Press any key to run the test...");
            Console.ReadKey();
            Console.Clear();

            DateTime start;
            Console.WriteLine($"Started at {start = DateTime.UtcNow}");

            Engine = new Engine();

            // Configure acceptor session
            var acceptor = new Configuration
            {
                Role = Role.Acceptor,
                Host = "127.0.0.1",
                Port = 1234,
                Sender = "TARGET",
                Target = "DAEV",
                InboundSeqNum = 1,
                OutboundSeqNum = 1,
                HeartbeatInterval = 5
            };

            // Configure initiator session
            var initiator = new Configuration
            {
                Role = Role.Initiator,
                Host = "127.0.0.1",
                Port = 1234,
                Sender = "DAEV",
                Target = "TARGET",
                InboundSeqNum = 1,
                OutboundSeqNum = 1,
                HeartbeatInterval = 5
            };

            // Create and start acceptor session on background thread
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Acceptor = Engine.Open(acceptor);
                    Acceptor.Logon();

                    Console.WriteLine("Acceptor logged on");

                    while (Running)
                    {
                        Acceptor.Receive();

                        var message = Acceptor.Outbound;

                        for (var i = 0; i < 100; i++)
                        {
                            message.Prepare("X");
                            message.Set(34, Acceptor.State.OutboundSeqNum);
                            message.Set(52, Acceptor.Clock.Time);
                            message.Set(49, Acceptor.Configuration.Sender);
                            message.Set(56, Acceptor.Configuration.Target);
                            message.Set(999, Acceptor.Clock.Time.Ticks);
                            message.Build();

                            Acceptor.Send(Acceptor.State, Acceptor.Channel, message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Acceptor failed because: {e.Message}");
                    Console.WriteLine();
                    Console.WriteLine(e);
                }
            }, TaskCreationOptions.LongRunning);

            Thread.Sleep(1000);

            // Start initiator session on background thread
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Initiator = Engine.Open(initiator);
                    Initiator.Logon();

                    Console.WriteLine("Initiator logged on");

                    while (Running)
                    {
                        Initiator.Receive();

                        if (Initiator.Inbound.Valid && Initiator.Inbound[35].Is("X"))
                        {
                            // Take measurement in microseconds - application send to application receive time (one way end-to-end)
                            Timings.Add((Initiator.Clock.Time.Ticks - Initiator.Inbound[999].AsLong) / 10);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Initiator failed because: {e.Message}");
                    Console.WriteLine();
                    Console.WriteLine(e);
                }
            }, TaskCreationOptions.LongRunning);

            Console.WriteLine();
            Console.WriteLine("Press any key to end the test...");
            Console.WriteLine();

            Thread.Sleep(3000);

            GC.Collect();
            GC.Collect();
            GC.Collect();

            var gc0 = GC.CollectionCount(0);
            var gc1 = GC.CollectionCount(1);
            var gc2 = GC.CollectionCount(2);

            Console.ReadLine();

            Running = false;

            Thread.Sleep(100);

            gc0 = GC.CollectionCount(0) - gc0;
            gc1 = GC.CollectionCount(1) - gc1;
            gc2 = GC.CollectionCount(2) - gc2;

            // Skip first few measurements (exclude jit and optimize passes)
            Timings = Timings.Skip(1000).ToList();

            Console.Clear();
            Console.WriteLine($"Count:    {Timings.Count}");
            Console.WriteLine($"Duration: {DateTime.UtcNow - start}");
            Console.WriteLine();
            Console.WriteLine($"End to end one-way");
            Console.WriteLine($"------------------");
            Console.WriteLine($"Latency max: {$"{Timings.Max():N}",14} micros");
            Console.WriteLine($"        avg: {$"{Timings.Average():N}",14} micros");
            Console.WriteLine($"        min: {$"{Timings.Min():N}",14} micros");
            Console.WriteLine();
            Console.WriteLine($"        99%: {$"{Percentile(Timings, 0.99):N}",14} micros");
            Console.WriteLine($"        90%: {$"{Percentile(Timings, 0.90):N}",14} micros");
            Console.WriteLine($"        50%: {$"{Percentile(Timings, 0.50):N}",14} micros");
            Console.WriteLine();
            Console.WriteLine($"        std: {$"{StdDev(Timings):N}",14} micros");
            Console.WriteLine();
            Console.WriteLine($"Garbage col:");
            Console.WriteLine($"         G0: {gc0}");
            Console.WriteLine($"         G1: {gc1}");
            Console.WriteLine($"         G2: {gc2}");
            Console.WriteLine();

            Console.WriteLine("Test finished, press any key to exit...");
            Console.ReadKey();
        }

        public static long Percentile(List<long> sequence, double excelPercentile)
        {
            var sorted = sequence.OrderBy(x => x).ToList();

            var N = sorted.Count();
            var n = (N - 1) * excelPercentile + 1;
            // Another method: double n = (N + 1) * excelPercentile; 
            if (n == 1d) return sorted[0];
            else if (n == N) return sorted[N - 1];
            else
            {
                var k = (int)n;
                var d = n - k;
                return (long)(sorted[k - 1] + d * (sorted[k] - sorted[k - 1]));
            }
        }

        public static double StdDev(List<long> values)
        {
            var ret = (double)0;
            var count = values.Count();

            if (count > 1)
            {
                // Compute the Average 
                var avg = values.Average();

                // Perform the Sum of (value-avg)^2 
                var sum = values.Sum(d => (d - avg) * (d - avg));

                // Put it all together 
                ret = Math.Sqrt(sum / count);
            }

            return ret;
        }
    }
}
