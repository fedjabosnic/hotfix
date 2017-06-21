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

        private readonly MessageWriter _message = new MessageWriter(1000, "FIX.4.2");

        [Benchmark]
        public void small()
        {
            _message.Prepare("0");
            _message.Set(34, 8059);
            _message.Set(52, _sendingTime);
            _message.Set(49, "SENDER....");
            _message.Set(56, "RECEIVER.....");

            _message.Build();
        }

        [Benchmark]
        public void medium()
        {
            _message.Prepare("8");
            _message.Set(34, 8059);
            _message.Set(52, _sendingTime);
            _message.Set(49, "SENDER....");
            _message.Set(56, "RECEIVER.....");
            _message.Set(20, 0);
            _message.Set(39, 2);
            _message.Set(150, 2);
            _message.Set(17, "U201:053117:00000079:B");
            _message.Set(40, 2);
            _message.Set(55, "EUR/CAD");
            _message.Set(54, 1);
            _message.Set(38, 900000); //f
            _message.Set(151, 0); //f
            _message.Set(14, 900000); //f
            _message.Set(32, 100000); //f
            _message.Set(32, 1); //f 1.503850
            _message.Set(6, 1); //f 1.503940
            _message.Set(64, _futSettleDate); // should be in YYYYMMDD format
            _message.Set(60, _transactTime);
            _message.Set(75, _tradeDate); // should be in YYYYMMDD format
            _message.Set(9200, 'S');
            _message.Set(9300, 647);
            _message.Set(9500, 0); //f
            _message.Set(37, "0804188884");
            _message.Set(15, "EUR");
            _message.Set(44, 1); //f 1.504200

            _message.Build();
        }

        [Benchmark]
        public void large()
        {
            _message.Prepare("0");
            _message.Set(34, 8059);
            _message.Set(52, _sendingTime);
            _message.Set(49, "SENDER....");
            _message.Set(56, "RECEIVER.....");
            _message.Set(262, "c6424b19-af74-4c17-8266-9c52ca583ad2");
            _message.Set(268, 8);

            _message.Set(279, 2);
            _message.Set(55, "GBP/JPY");
            _message.Set(269, 0);
            _message.Set(278, "1211918436");
            _message.Set(270, 144); //f 144.808000
            _message.Set(271, 1000000); //f
            _message.Set(110, 0); //f
            _message.Set(15, "GBP");
            _message.Set(282, 290);

            _message.Set(279, 2);
            _message.Set(55, "GBP/JPY");
            _message.Set(269, 0);
            _message.Set(278, "1211918437");
            _message.Set(270, 144); //f 144.802000
            _message.Set(271, 2000000); //f
            _message.Set(110, 0); //f
            _message.Set(15, "GBP");
            _message.Set(282, 290);

            _message.Set(279, 0);
            _message.Set(55, "GBP/JPY");
            _message.Set(269, 0);
            _message.Set(278, "1211918501");
            _message.Set(270, 144); //f 144.808000
            _message.Set(271, 1000000); //f
            _message.Set(110, 0); //f
            _message.Set(15, "GBP");
            _message.Set(282, 290);
            _message.Set(735, 1);
            _message.Set(695, 5);

            _message.Set(279, 0);
            _message.Set(55, "GBP/JPY");
            _message.Set(269, 0);
            _message.Set(278, "1211918502");
            _message.Set(270, 144); //f 144.808000
            _message.Set(271, 1000000); //f
            _message.Set(110, 0); //f
            _message.Set(15, "GBP");
            _message.Set(282, 290);
            _message.Set(735, 1);
            _message.Set(695, 5);

            _message.Set(279, 2);
            _message.Set(55, "GBP/JPY");
            _message.Set(269, 0);
            _message.Set(278, "1211918436");
            _message.Set(270, 144); //f 144.808000
            _message.Set(271, 1000000); //f
            _message.Set(110, 0); //f
            _message.Set(15, "GBP");
            _message.Set(282, 290);

            _message.Set(279, 2);
            _message.Set(55, "GBP/JPY");
            _message.Set(269, 0);
            _message.Set(278, "1211918436");
            _message.Set(270, 144); //f 144.808000
            _message.Set(271, 1000000); //f
            _message.Set(110, 0); //f
            _message.Set(15, "GBP");
            _message.Set(282, 290);

            _message.Set(279, 2);
            _message.Set(55, "GBP/JPY");
            _message.Set(269, 0);
            _message.Set(278, "1211918436");
            _message.Set(270, 144); //f 144.808000
            _message.Set(271, 1000000); //f
            _message.Set(110, 0); //f
            _message.Set(15, "GBP");
            _message.Set(282, 290);
            _message.Set(735, 1);
            _message.Set(695, 5);

            _message.Set(279, 2);
            _message.Set(55, "GBP/JPY");
            _message.Set(269, 0);
            _message.Set(278, "1211918436");
            _message.Set(270, 144); //f 144.808000
            _message.Set(271, 1000000); //f
            _message.Set(110, 0); //f
            _message.Set(15, "GBP");
            _message.Set(282, 290);
            _message.Set(735, 1);
            _message.Set(695, 5);

            _message.Build();
        }
    }
}
