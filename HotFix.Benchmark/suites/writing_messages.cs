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
        private readonly DateTime _sendingTime = DateTime.Parse("2017/05/31 08:18:01.768");
        private readonly DateTime _futSettleDate = DateTime.Parse("2017/06/02");
        private readonly DateTime _transactTime = DateTime.Parse("2017/05/31 08:18:01.767");
        private readonly DateTime _tradeDate = DateTime.Parse("2017/05/31");

        [Benchmark]
        public void small()
        {
            var message = new MessageWriter(1000, "FIX.4.2");

            message.Prepare("0");
            message.Set(34, 8059);
            message.Set(52, _sendingTime);
            message.Set(49, "SENDER....");
            message.Set(56, "RECEIVER.....");

            message.Build();
        }

        [Benchmark]
        public void medium()
        {
            var message = new MessageWriter(1000, "FIX.4.2");

            message.Prepare("8");
            message.Set(34, 8059);
            message.Set(52, _sendingTime);
            message.Set(49, "SENDER....");
            message.Set(56, "RECEIVER.....");
            message.Set(20, 0);
            message.Set(39, 2);
            message.Set(150, 2);
            message.Set(17, "U201:053117:00000079:B");
            message.Set(40, 2);
            message.Set(55, "EUR/CAD");
            message.Set(54, 1);
            message.Set(38, 900000); //f
            message.Set(151, 0); //f
            message.Set(14, 900000); //f
            message.Set(32, 100000); //f
            message.Set(32, 1); //f 1.503850
            message.Set(6, 1); //f 1.503940
            message.Set(64, _futSettleDate); // should be in YYYYMMDD format
            message.Set(60, _transactTime);
            message.Set(75, _tradeDate); // should be in YYYYMMDD format
            message.Set(9200, 'S');
            message.Set(9300, 647);
            message.Set(9500, 0); //f
            message.Set(37, "0804188884");
            message.Set(15, "EUR");
            message.Set(44, 1); //f 1.504200

            message.Build();
        }

        [Benchmark]
        public void large()
        {
            var message = new MessageWriter(1000, "FIX.4.2");

            message.Prepare("0");
            message.Set(34, 8059);
            message.Set(52, _sendingTime);
            message.Set(49, "SENDER....");
            message.Set(56, "RECEIVER.....");
            message.Set(262, "c6424b19-af74-4c17-8266-9c52ca583ad2");
            message.Set(268, 8);

            message.Set(279, 2);
            message.Set(55, "GBP/JPY");
            message.Set(269, 0);
            message.Set(278, "1211918436");
            message.Set(270, 144); //f 144.808000
            message.Set(271, 1000000); //f
            message.Set(110, 0); //f
            message.Set(15, "GBP");
            message.Set(282, 290);

            message.Set(279, 2);
            message.Set(55, "GBP/JPY");
            message.Set(269, 0);
            message.Set(278, "1211918437");
            message.Set(270, 144); //f 144.802000
            message.Set(271, 2000000); //f
            message.Set(110, 0); //f
            message.Set(15, "GBP");
            message.Set(282, 290);

            message.Set(279, 0);
            message.Set(55, "GBP/JPY");
            message.Set(269, 0);
            message.Set(278, "1211918501");
            message.Set(270, 144); //f 144.808000
            message.Set(271, 1000000); //f
            message.Set(110, 0); //f
            message.Set(15, "GBP");
            message.Set(282, 290);
            message.Set(735, 1);
            message.Set(695, 5);

            message.Set(279, 0);
            message.Set(55, "GBP/JPY");
            message.Set(269, 0);
            message.Set(278, "1211918502");
            message.Set(270, 144); //f 144.808000
            message.Set(271, 1000000); //f
            message.Set(110, 0); //f
            message.Set(15, "GBP");
            message.Set(282, 290);
            message.Set(735, 1);
            message.Set(695, 5);

            message.Set(279, 2);
            message.Set(55, "GBP/JPY");
            message.Set(269, 0);
            message.Set(278, "1211918436");
            message.Set(270, 144); //f 144.808000
            message.Set(271, 1000000); //f
            message.Set(110, 0); //f
            message.Set(15, "GBP");
            message.Set(282, 290);

            message.Set(279, 2);
            message.Set(55, "GBP/JPY");
            message.Set(269, 0);
            message.Set(278, "1211918436");
            message.Set(270, 144); //f 144.808000
            message.Set(271, 1000000); //f
            message.Set(110, 0); //f
            message.Set(15, "GBP");
            message.Set(282, 290);

            message.Set(279, 2);
            message.Set(55, "GBP/JPY");
            message.Set(269, 0);
            message.Set(278, "1211918436");
            message.Set(270, 144); //f 144.808000
            message.Set(271, 1000000); //f
            message.Set(110, 0); //f
            message.Set(15, "GBP");
            message.Set(282, 290);
            message.Set(735, 1);
            message.Set(695, 5);

            message.Set(279, 2);
            message.Set(55, "GBP/JPY");
            message.Set(269, 0);
            message.Set(278, "1211918436");
            message.Set(270, 144); //f 144.808000
            message.Set(271, 1000000); //f
            message.Set(110, 0); //f
            message.Set(15, "GBP");
            message.Set(282, 290);
            message.Set(735, 1);
            message.Set(695, 5);

            message.Build();
        }
    }
}
