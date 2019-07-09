using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using HdrHistogram;
using HotFix.Core;

namespace HotFix.Demo.Initiator
{
    /// <summary>
    /// This demo is an initiator implemented with the lower level api by directly manipulating the session. It will
    /// connect to the specified acceptor and send the specified number of orders, measuring the round trip time for
    /// trading including transport overheads.
    /// </summary>
    class Program
    {
        public static LongHistogram Rtt;
        public static LongHistogram Encode;
        public static LongHistogram Decode;

        public static int GC0;
        public static int GC1;
        public static int GC2;

        static void Main(string[] args)
        {
            Console.WriteLine();

            var host = System.Net.Dns.GetHostAddresses(args[0])[0].ToString();
            var port = int.Parse(args[1]);
            var count = int.Parse(args[2]);

            Rtt = new LongHistogram(1, 10000000, 5);
            Encode = new LongHistogram(1, 10000000, 5);
            Decode = new LongHistogram(1, 10000000, 5);

            var engine = new Engine();

            var configuration = new Configuration
            {
                Role = Role.Initiator,
                Version = "FIX.4.2",
                Host = host,
                Port = port,
                Sender = "Client",
                Target = "Server",
                InboundSeqNum = 1,
                OutboundSeqNum = 1,
                HeartbeatInterval = 0,
                //LogFile = @"messages.log" // Enable to see logging impact
            };

            using (var initiator = engine.Open(configuration))
            {
                initiator.Logon();

                Console.WriteLine("Logged on");

                GC.Collect(0, GCCollectionMode.Forced, true, true);
                GC.Collect(1, GCCollectionMode.Forced, true, true);
                GC.Collect(2, GCCollectionMode.Forced, true, true);

                GC0 = GC.CollectionCount(0);
                GC1 = GC.CollectionCount(1);
                GC2 = GC.CollectionCount(2);

                //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Thread.BeginThreadAffinity();

                for (var i = 0; i < count; i++)
                {
                    var time = initiator.Clock.Time;

                    var order = initiator
                        .Outbound
                        .Clear()
                        .Set(60, time)       // TransactTime
                        .Set(11, time.Ticks) // ClOrdId
                        .Set(55, "EUR/USD")  // Symbol
                        .Set(54, 1)          // Side (buy)
                        .Set(38, 1000.00)    // OrderQty
                        .Set(44, 1.13200)    // Price
                        .Set(40, 2);         // OrdType (limit)

                    // Send order
                    initiator.Send("D", order);

                    // Wait for an execution report (ignore everything else)
                    while (!initiator.Receive() && !initiator.Inbound[35].Is("8")) { }

                    var rtt = initiator.Clock.Time.Ticks - time.Ticks;

                    if (i < 1000) continue;

                    // Update statistics
                    Rtt.RecordValue(rtt);
                    Encode.RecordValue(initiator.Outbound.Duration);
                    Decode.RecordValue(initiator.Inbound.Duration);
                }

                GC0 = GC.CollectionCount(0) - GC0;
                GC1 = GC.CollectionCount(1) - GC1;
                GC2 = GC.CollectionCount(2) - GC2;

                Thread.EndThreadAffinity();

                initiator.Logout();

                Console.WriteLine("Logged out");
            }

            PrintStatistics();
            SaveStatistics();
        }

        private static void PrintStatistics()
        {
            Console.WriteLine();
            Console.WriteLine($"|         |    Round trip |        Encode |        Decode |");
            Console.WriteLine($"|---------|---------------|---------------|---------------|");
            Console.WriteLine($"|     min | {$"{Rtt.GetValueAtPercentile( 00.00) / 10m:N}",10} µs | {$"{Encode.GetValueAtPercentile( 00.00) / 10m:N}",10} µs | {$"{Decode.GetValueAtPercentile( 00.00) / 10m:N}",10} µs |");
            Console.WriteLine($"|  50.00% | {$"{Rtt.GetValueAtPercentile( 50.00) / 10m:N}",10} µs | {$"{Encode.GetValueAtPercentile( 50.00) / 10m:N}",10} µs | {$"{Decode.GetValueAtPercentile( 50.00) / 10m:N}",10} µs |");
            Console.WriteLine($"|  90.00% | {$"{Rtt.GetValueAtPercentile( 90.00) / 10m:N}",10} µs | {$"{Encode.GetValueAtPercentile( 90.00) / 10m:N}",10} µs | {$"{Decode.GetValueAtPercentile( 90.00) / 10m:N}",10} µs |");
            Console.WriteLine($"|  99.00% | {$"{Rtt.GetValueAtPercentile( 99.00) / 10m:N}",10} µs | {$"{Encode.GetValueAtPercentile( 99.00) / 10m:N}",10} µs | {$"{Decode.GetValueAtPercentile( 99.00) / 10m:N}",10} µs |");
            Console.WriteLine($"|  99.90% | {$"{Rtt.GetValueAtPercentile( 99.90) / 10m:N}",10} µs | {$"{Encode.GetValueAtPercentile( 99.90) / 10m:N}",10} µs | {$"{Decode.GetValueAtPercentile( 99.90) / 10m:N}",10} µs |");
            Console.WriteLine($"|  99.99% | {$"{Rtt.GetValueAtPercentile( 99.99) / 10m:N}",10} µs | {$"{Encode.GetValueAtPercentile( 99.99) / 10m:N}",10} µs | {$"{Decode.GetValueAtPercentile( 99.99) / 10m:N}",10} µs |");
            Console.WriteLine($"|     max | {$"{Rtt.GetValueAtPercentile(100.00) / 10m:N}",10} µs | {$"{Encode.GetValueAtPercentile(100.00) / 10m:N}",10} µs | {$"{Decode.GetValueAtPercentile(100.00) / 10m:N}",10} µs |");
            Console.WriteLine();
            Console.WriteLine($"GC 0: {GC0}");
            Console.WriteLine($"GC 1: {GC1}");
            Console.WriteLine($"GC 2: {GC2}");
        }

        private static void SaveStatistics()
        {
            Directory.CreateDirectory(@".bench");
            using (var writer = new StreamWriter(@".bench/histogram-rtt.hgrm")) Rtt.OutputPercentileDistribution(writer, outputValueUnitScalingRatio: 10);
            using (var writer = new StreamWriter(@".bench/histogram-encode.hgrm")) Encode.OutputPercentileDistribution(writer, outputValueUnitScalingRatio: 10);
            using (var writer = new StreamWriter(@".bench/histogram-decode.hgrm")) Decode.OutputPercentileDistribution(writer, outputValueUnitScalingRatio: 10);
        }
    }
}
