using System;
using System.Collections.Generic;
using System.Linq;
using HotFix.Core;

namespace HotFix.Demo.Initiator
{
    class Program
    {
        private static int _orders;

        static void Main(string[] args)
        {
            Console.WriteLine("Sending orders...");

            var count = int.Parse(args[2]);
            var histogram = new List<long>(count * 2);

            var engine = new Engine();

            var configuration = new Configuration
            {
                Role = Role.Initiator,
                Version = "FIX.4.2",
                Host = args[0],
                Port = int.Parse(args[1]),
                Sender = "Client",
                Target = "Server",
                InboundSeqNum = 1,
                OutboundSeqNum = 1,
                HeartbeatInterval = 86400,
                //LogFile = @"messages.log" // Enable to see logging impact
            };

            using (var session = engine.Open(configuration))
            {
                var clock = session.Clock;

                var inbound = session.Inbound;
                var outbound = session.Outbound;

                session.Logon();

                while (session.Active && _orders < count)
                {
                    var sent = clock.Time;

                    outbound
                        .Clear()
                        .Set(60, clock.Time)         // TransactTime 
                        .Set(11, ++_orders)          // ClOrdId 
                        .Set(55, "EUR/USD")          // Symbol 
                        .Set(54, 1)                  // Side (buy) 
                        .Set(38, 1000.00)            // OrderQty 
                        .Set(44, 1.13200)            // Price 
                        .Set(40, 2);                 // OrdType (limit) 

                    session.Send("D", outbound);

                    while (!session.Receive()) { }

                    if (inbound[35].Is("8"))
                    {
                        var received = clock.Time;

                        histogram.Add(received.Ticks - sent.Ticks);
                    }
                }

                session.Logout();
            }

            Console.WriteLine();
            Console.WriteLine("Orders: " + _orders);

            histogram = histogram.Skip(1000).Select(x => x).ToList();

            Console.WriteLine($"      min: {$"{histogram.Min() / 10m:N}",14} µs");
            Console.WriteLine($"   50.00%: {$"{Percentile(histogram, 0.5000) / 10m:N}",14} µs");
            Console.WriteLine($"   90.00%: {$"{Percentile(histogram, 0.9000) / 10m:N}",14} µs");
            Console.WriteLine($"   99.00%: {$"{Percentile(histogram, 0.9900) / 10m:N}",14} µs");
            Console.WriteLine($"   99.90%: {$"{Percentile(histogram, 0.9990) / 10m:N}",14} µs");
            Console.WriteLine($"   99.99%: {$"{Percentile(histogram, 0.9999) / 10m:N}",14} µs");
            Console.WriteLine($"      max: {$"{histogram.Max() / 10m:N}",14} µs");
        }

        public static long Percentile(List<long> sequence, double excelPercentile)
        {
            var sorted = sequence.OrderBy(x => x).ToList();

            var N = sorted.Count;
            var n = (N - 1) * excelPercentile + 1;

            if (n == 1d) return sorted[0];
            if (n == N) return sorted[N - 1];

            var k = (int)n;
            var d = n - k;

            return (long)(sorted[k - 1] + d * (sorted[k] - sorted[k - 1]));
        }
    }
}
