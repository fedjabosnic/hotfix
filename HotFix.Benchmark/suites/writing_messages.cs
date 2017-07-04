using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using HotFix.Core;

namespace HotFix.Benchmark.suites
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000)]
    public class writing_messages
    {
        public readonly DateTime SendingTime = DateTime.ParseExact("2017/05/31 08:18:01.768", "yyyy/MM/dd HH:mm:ss.fff", null);
        public readonly DateTime FutSettleDate = DateTime.ParseExact("2017/06/02", "yyyy/MM/dd", null);
        public readonly DateTime TransactTime = DateTime.ParseExact("2017/05/31 08:18:01.767", "yyyy/MM/dd", null);
        public readonly DateTime TradeDate = DateTime.ParseExact("2017/05/31", "yyyy/MM/dd", null);

        public readonly FIXMessageWriter Message = new FIXMessageWriter(1000, "FIX.4.2");

        [Benchmark]
        public void small()
        {
            Message.Prepare("0");
            Message.Set(34, 8059);
            Message.Set(52, SendingTime);
            Message.Set(49, "SENDER....");
            Message.Set(56, "RECEIVER.....");

            Message.Build();
        }

        [Benchmark]
        public void medium()
        {
            Message.Prepare("8");
            Message.Set(34, 8059);
            Message.Set(52, SendingTime);
            Message.Set(49, "SENDER....");
            Message.Set(56, "RECEIVER.....");
            Message.Set(20, 0);
            Message.Set(39, 2);
            Message.Set(150, 2);
            Message.Set(17, "U201:053117:00000079:B");
            Message.Set(40, 2);
            Message.Set(55, "EUR/CAD");
            Message.Set(54, 1);
            Message.Set(38, 900000d);
            Message.Set(151, 0);
            Message.Set(14, 900000d);
            Message.Set(32, 100000d);
            Message.Set(32, 1.503850d);
            Message.Set(6, 1.503940);
            Message.Set(64, FutSettleDate); // should be in YYYYMMDD format
            Message.Set(60, TransactTime);
            Message.Set(75, TradeDate); // should be in YYYYMMDD format
            Message.Set(9200, 'S');
            Message.Set(9300, 647);
            Message.Set(9500, 0d);
            Message.Set(37, "0804188884");
            Message.Set(15, "EUR");
            Message.Set(44, 1.504200d);

            Message.Build();
        }

        [Benchmark]
        public void large()
        {
            Message.Prepare("0");
            Message.Set(34, 8059);
            Message.Set(52, SendingTime);
            Message.Set(49, "SENDER....");
            Message.Set(56, "RECEIVER.....");
            Message.Set(262, "c6424b19-af74-4c17-8266-9c52ca583ad2");
            Message.Set(268, 8);

            Message.Set(279, 2);
            Message.Set(55, "GBP/JPY");
            Message.Set(269, 0);
            Message.Set(278, "1211918436");
            Message.Set(270, 144.808000d);
            Message.Set(271, 1000000d);
            Message.Set(110, 0d);
            Message.Set(15, "GBP");
            Message.Set(282, 290);

            Message.Set(279, 2);
            Message.Set(55, "GBP/JPY");
            Message.Set(269, 0);
            Message.Set(278, "1211918437");
            Message.Set(270, 144.802000d);
            Message.Set(271, 2000000d);
            Message.Set(110, 0d);
            Message.Set(15, "GBP");
            Message.Set(282, 290);

            Message.Set(279, 0);
            Message.Set(55, "GBP/JPY");
            Message.Set(269, 0);
            Message.Set(278, "1211918501");
            Message.Set(270, 144.808000d);
            Message.Set(271, 1000000d);
            Message.Set(110, 0d);
            Message.Set(15, "GBP");
            Message.Set(282, 290);
            Message.Set(735, 1);
            Message.Set(695, 5);

            Message.Set(279, 0);
            Message.Set(55, "GBP/JPY");
            Message.Set(269, 0);
            Message.Set(278, "1211918502");
            Message.Set(270, 144.808000d);
            Message.Set(271, 1000000d);
            Message.Set(110, 0d);
            Message.Set(15, "GBP");
            Message.Set(282, 290);
            Message.Set(735, 1);
            Message.Set(695, 5);

            Message.Set(279, 2);
            Message.Set(55, "GBP/JPY");
            Message.Set(269, 0);
            Message.Set(278, "1211918436");
            Message.Set(270, 144.808000d);
            Message.Set(271, 1000000d);
            Message.Set(110, 0d);
            Message.Set(15, "GBP");
            Message.Set(282, 290);

            Message.Set(279, 2);
            Message.Set(55, "GBP/JPY");
            Message.Set(269, 0);
            Message.Set(278, "1211918436");
            Message.Set(270, 144.808000d);
            Message.Set(271, 1000000d);
            Message.Set(110, 0d);
            Message.Set(15, "GBP");
            Message.Set(282, 290);

            Message.Set(279, 2);
            Message.Set(55, "GBP/JPY");
            Message.Set(269, 0);
            Message.Set(278, "1211918436");
            Message.Set(270, 144.808000d);
            Message.Set(271, 1000000d);
            Message.Set(110, 0d);
            Message.Set(15, "GBP");
            Message.Set(282, 290);
            Message.Set(735, 1);
            Message.Set(695, 5);

            Message.Set(279, 2);
            Message.Set(55, "GBP/JPY");
            Message.Set(269, 0);
            Message.Set(278, "1211918436");
            Message.Set(270, 144.808000d);
            Message.Set(271, 1000000d);
            Message.Set(110, 0d);
            Message.Set(15, "GBP");
            Message.Set(282, 290);
            Message.Set(735, 1);
            Message.Set(695, 5);

            Message.Build();
        }
    }
}
