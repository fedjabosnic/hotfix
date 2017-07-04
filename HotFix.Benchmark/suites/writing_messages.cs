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
            Message
                .Prepare("0")
                .Set(34, 8059)
                .Set(52, SendingTime)
                .Set(49, "SENDER....")
                .Set(56, "RECEIVER.....")
                .Build();
        }

        [Benchmark]
        public void medium()
        {
            Message
                .Prepare("8")
                .Set(34, 8059)
                .Set(52, SendingTime)
                .Set(49, "SENDER....")
                .Set(56, "RECEIVER.....")
                .Set(20, 0)
                .Set(39, 2)
                .Set(150, 2)
                .Set(17, "U201:053117:00000079:B")
                .Set(40, 2)
                .Set(55, "EUR/CAD")
                .Set(54, 1)
                .Set(38, 900000d)
                .Set(151, 0)
                .Set(14, 900000d)
                .Set(32, 100000d)
                .Set(32, 1.503850d)
                .Set(6, 1.503940)
                .Set(64, FutSettleDate) // should be in YYYYMMDD format
                .Set(60, TransactTime)
                .Set(75, TradeDate) // should be in YYYYMMDD format
                .Set(9200, 'S')
                .Set(9300, 647)
                .Set(9500, 0d)
                .Set(37, "0804188884")
                .Set(15, "EUR")
                .Set(44, 1.504200d)
                .Build();
        }

        [Benchmark]
        public void large()
        {
            Message
                .Prepare("0")
                .Set(34, 8059)
                .Set(52, SendingTime)
                .Set(49, "SENDER....")
                .Set(56, "RECEIVER.....")
                .Set(262, "c6424b19-af74-4c17-8266-9c52ca583ad2")
                .Set(268, 8);

            Message
                .Set(279, 2)
                .Set(55, "GBP/JPY")
                .Set(269, 0)
                .Set(278, "1211918436")
                .Set(270, 144.808000d)
                .Set(271, 1000000d)
                .Set(110, 0d)
                .Set(15, "GBP")
                .Set(282, 290);

            Message
                .Set(279, 2)
                .Set(55, "GBP/JPY")
                .Set(269, 0)
                .Set(278, "1211918437")
                .Set(270, 144.802000d)
                .Set(271, 2000000d)
                .Set(110, 0d)
                .Set(15, "GBP")
                .Set(282, 290);

            Message
                .Set(279, 0)
                .Set(55, "GBP/JPY")
                .Set(269, 0)
                .Set(278, "1211918501")
                .Set(270, 144.808000d)
                .Set(271, 1000000d)
                .Set(110, 0d)
                .Set(15, "GBP")
                .Set(282, 290)
                .Set(735, 1)
                .Set(695, 5);

            Message.Set(279, 0)
                .Set(55, "GBP/JPY")
                .Set(269, 0)
                .Set(278, "1211918502")
                .Set(270, 144.808000d)
                .Set(271, 1000000d)
                .Set(110, 0d)
                .Set(15, "GBP")
                .Set(282, 290)
                .Set(735, 1)
                .Set(695, 5);

            Message.Set(279, 2)
                .Set(55, "GBP/JPY")
                .Set(269, 0)
                .Set(278, "1211918436")
                .Set(270, 144.808000d)
                .Set(271, 1000000d)
                .Set(110, 0d)
                .Set(15, "GBP")
                .Set(282, 290);

            Message.Set(279, 2)
                .Set(55, "GBP/JPY")
                .Set(269, 0)
                .Set(278, "1211918436")
                .Set(270, 144.808000d)
                .Set(271, 1000000d)
                .Set(110, 0d)
                .Set(15, "GBP")
                .Set(282, 290);

            Message.Set(279, 2)
                .Set(55, "GBP/JPY")
                .Set(269, 0)
                .Set(278, "1211918436")
                .Set(270, 144.808000d)
                .Set(271, 1000000d)
                .Set(110, 0d)
                .Set(15, "GBP")
                .Set(282, 290)
                .Set(735, 1)
                .Set(695, 5);

            Message.Set(279, 2)
                .Set(55, "GBP/JPY")
                .Set(269, 0)
                .Set(278, "1211918436")
                .Set(270, 144.808000d)
                .Set(271, 1000000d)
                .Set(110, 0d)
                .Set(15, "GBP")
                .Set(282, 290)
                .Set(735, 1)
                .Set(695, 5);

            Message.Build();
        }
    }
}
