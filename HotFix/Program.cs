using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using HotFix.Core;
using HotFix.Transport;
using HotFix.Utilities;

namespace HotFix
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new Configuration
            {
                Host = "localhost",
                Port = 1234,
                Sender = "DAEV",
                Target = "TARGET",
                InboundSeqNum = 0,
                OutboundSeqNum = 0,
                HeartbeatInterval = 5,
                InboundBufferSize = 65536,
                OutboundBufferSize = 65536
            };

            var transport = new TcpTransport(configuration.Host, configuration.Port);
            var session = new Session(transport, configuration);

            session.Run();

            return;
            Console.WriteLine("Preparing...");

            var count = 1000000;
            var results = new List<DateTime>(count);

            var throwaway1 = "20170327 15:33:21.596".GetDateTime();
            var throwaway2 = "20170327 07:45:18.123".GetDateTime();
            var throwaway3 = "20170327 12:37:13.645".GetDateTime();
            var throwaway4 = "20170327 14:12:32.566".GetDateTime();
            var throwaway5 = "20170327 21:04:37.236".GetDateTime();

            var report = "8=FIX.4.2|9=396|35=8|34=000008059|52=20170531-08:18:01.768|49=SENDER....|56=RECEIVER.....|20=0|39=2|150=2|17=U201:053117:00000079:B|40=2|55=EUR/CAD|54=1|38=000900000.00|151=000000000.00|14=000900000.00|32=000100000.00|31=00001.503850|6=00001.503940|64=20170602|60=20170531-08:18:01.767|75=20170531|9200=S|9300=0647|9500=00000.000000|37=0804188884|15=EUR|44=00001.504200|375=None|11=20170531.FXALGO0900.001.00011|10=241|".Replace("|", "\u0001");
            var md = "8=FIX.4.2|9=968|35=X|34=53677|52=20170525-00:55:16.153|49=SENDER..|56=RECEIVER..........|262=c6424b19-af74-4c17-8266-9c52ca583ad2|268=8|279=2|55=GBP/JPY|269=0|278=1211918436|270=144.808000|271=1000000.000000|110=0.000000|15=GBP|282=290|279=2|55=GBP/JPY|269=0|278=1211918437|270=144.802000|271=2000000.000000|110=0.000000|15=GBP|282=290|279=0|55=GBP/JPY|269=0|278=1211918501|270=144.809000|271=1000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5|279=0|55=GBP/JPY|269=0|278=1211918502|270=144.803000|271=2000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5|279=2|55=GBP/JPY|269=1|278=1211918438|270=144.826000|271=1000000.000000|110=0.000000|15=GBP|282=290|279=2|55=GBP/JPY|269=1|278=1211918439|270=144.833000|271=2000000.000000|110=0.000000|15=GBP|282=290|279=0|55=GBP/JPY|269=1|278=1211918503|270=144.828000|271=1000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5|279=0|55=GBP/JPY|269=1|278=1211918504|270=144.834000|271=2000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5|10=161|".Replace("|","\u0001");

            var message = new Message();

            Console.WriteLine("Press any key to run test...");
            Console.WriteLine("");
            Console.ReadLine();

            GC.Collect();
            Thread.Sleep(100);

            var c0 = GC.CollectionCount(0);
            var c1 = GC.CollectionCount(1);
            var c2 = GC.CollectionCount(2);

            var timer = new Stopwatch();

            timer.Start();

            for (var i = 0; i < count; i++)
            {
                // Parse message and retrieve timestamp field
                results.Add(message.Parse(md)[52].DateTime);
            }

            timer.Stop();

            Console.WriteLine("Done");

            Console.WriteLine();
            Console.WriteLine($"Entries: {count}");
            Console.WriteLine($"Elapsed: {timer.ElapsedMilliseconds} millis");
            Console.WriteLine($"Rates:   {count / ((double)timer.ElapsedTicks / Stopwatch.Frequency)} entries per second");
            Console.WriteLine($"         {((double)timer.ElapsedTicks / Stopwatch.Frequency) / count * 1000000} micros per entry");
            Console.WriteLine();
            Console.WriteLine($"Collections:");
            Console.WriteLine($"      Gen 0: {GC.CollectionCount(0) - c0}");
            Console.WriteLine($"      Gen 1: {GC.CollectionCount(1) - c1}");
            Console.WriteLine($"      Gen 2: {GC.CollectionCount(2) - c2}");
            Console.WriteLine();
            Console.WriteLine(throwaway1);
            Console.WriteLine(throwaway2);
            Console.WriteLine(throwaway3);
            Console.WriteLine(throwaway4);
            Console.WriteLine(throwaway5);
            Console.WriteLine(results[12345]);
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");

            Console.ReadLine();
        }
    }

    
}
