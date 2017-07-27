using System;
using System.Collections.Generic;
using System.Linq;
using HotFix.Core;

namespace HotFix.Demo.Initiator
{
    /// <summary>
    /// This demo is an initiator implemented with the lower level api by directly manipulating the session. It will
    /// connect to the specified acceptor and send the specified number of orders, measuring the end-to-end time for
    /// trading including transport overheads.
    /// </summary>
    class Program
    {
        public static List<long> Histogram;

        static void Main(string[] args)
        {
            Console.WriteLine();

            var host = args[0];
            var port = int.Parse(args[1]);
            var count = int.Parse(args[2]);

            Histogram = new List<long>(count * 2);

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

                for (var i = 0; i < count; i++)
                {
                    var order = initiator
                        .Outbound
                        .Clear()
                        .Set(60, initiator.Clock.Time)       // TransactTime
                        .Set(11, initiator.Clock.Time.Ticks) // ClOrdId
                        .Set(55, "EUR/USD")                  // Symbol
                        .Set(54, 1)                          // Side (buy)
                        .Set(38, 1000.00)                    // OrderQty
                        .Set(44, 1.13200)                    // Price
                        .Set(40, 2);                         // OrdType (limit)

                    // Send order
                    initiator.Send("D", order);

                    // Wait for an execution report (ignore everything else)
                    while (!initiator.Receive() && !initiator.Inbound[35].Is("8")) { }

                    // Update statistics
                    Histogram.Add(initiator.Clock.Time.Ticks - initiator.Inbound[11].AsLong);
                }

                initiator.Logout();

                Console.WriteLine("Logged out");
            }


            PrintStatistics();
        }

        private static void PrintStatistics()
        {
            Console.WriteLine("Orders: " + Histogram.Count);

            Histogram = Histogram.Skip(1000).Select(x => x).ToList();

            Console.WriteLine($"      min: {$"{Histogram.Min() / 10m:N}",14} µs");
            Console.WriteLine($"   50.00%: {$"{Percentile(Histogram, 0.5000m) / 10m:N}",14} µs");
            Console.WriteLine($"   90.00%: {$"{Percentile(Histogram, 0.9000m) / 10m:N}",14} µs");
            Console.WriteLine($"   99.00%: {$"{Percentile(Histogram, 0.9900m) / 10m:N}",14} µs");
            Console.WriteLine($"   99.90%: {$"{Percentile(Histogram, 0.9990m) / 10m:N}",14} µs");
            Console.WriteLine($"   99.99%: {$"{Percentile(Histogram, 0.9999m) / 10m:N}",14} µs");
            Console.WriteLine($"      max: {$"{Histogram.Max() / 10m:N}",14} µs");
        }

        public static long Percentile(List<long> sequence, decimal excelPercentile)
        {
            var sorted = sequence.OrderBy(x => x).ToList();

            var N = sorted.Count;
            var n = (N - 1) * excelPercentile + 1;

            if (n == 1m) return sorted[0];
            if (n == N) return sorted[N - 1];

            var k = (int)n;
            var d = n - k;

            return (long)(sorted[k - 1] + d * (sorted[k] - sorted[k - 1]));
        }
    }
}
