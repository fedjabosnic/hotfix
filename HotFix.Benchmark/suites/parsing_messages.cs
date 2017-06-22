using System;
using System.Text;
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
    public class parsing_messages
    {
        private byte[] _heartbeat;
        private byte[] _executionReport;
        private byte[] _marketDataIncrementalRefresh;

        public Message Message { get; set; }

        [Setup]
        public void Setup()
        {
            Message = new Message();

            _heartbeat =
                Encoding.ASCII.GetBytes(
                    ("8=FIX.4.2|9=74|35=8|34=000008059|52=20170531-08:18:01.768|49=SENDER....|56=RECEIVER....." +
                    "|10=048|")
                .Replace("|", "\u0001"));

            _executionReport =
                Encoding.ASCII.GetBytes(
                    ("8=FIX.4.2|9=354|35=8|34=000008059|52=20170531-08:18:01.768|49=SENDER....|56=RECEIVER.....|20=0|39=2|150=2" +
                    "|17=U201:053117:00000079:B|40=2|55=EUR/CAD|54=1|38=000900000.00|151=000000000.00|14=000900000.00|32=000100000.00|31=00001.503850" +
                    "|6=00001.503940|64=20170602|60=20170531-08:18:01.767|75=20170531|9200=S|9300=0647|9500=00000.000000|37=0804188884|15=EUR|44=00001.504200" +
                    "|10=035|")
                .Replace("|", "\u0001"));

            _marketDataIncrementalRefresh = 
                Encoding.ASCII.GetBytes(
                    ("8=FIX.4.2|9=968|35=X|34=53677|52=20170525-00:55:16.153|49=SENDER..|56=RECEIVER..........|262=c6424b19-af74-4c17-8266-9c52ca583ad2" +
                    "|268=8" +
                    "|279=2|55=GBP/JPY|269=0|278=1211918436|270=144.808000|271=1000000.000000|110=0.000000|15=GBP|282=290" +
                    "|279=2|55=GBP/JPY|269=0|278=1211918437|270=144.802000|271=2000000.000000|110=0.000000|15=GBP|282=290" +
                    "|279=0|55=GBP/JPY|269=0|278=1211918501|270=144.809000|271=1000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                    "|279=0|55=GBP/JPY|269=0|278=1211918502|270=144.803000|271=2000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                    "|279=2|55=GBP/JPY|269=1|278=1211918438|270=144.826000|271=1000000.000000|110=0.000000|15=GBP|282=290" +
                    "|279=2|55=GBP/JPY|269=1|278=1211918439|270=144.833000|271=2000000.000000|110=0.000000|15=GBP|282=290" +
                    "|279=0|55=GBP/JPY|269=1|278=1211918503|270=144.828000|271=1000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                    "|279=0|55=GBP/JPY|269=1|278=1211918504|270=144.834000|271=2000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                    "|10=049|")
                .Replace("|", "\u0001"));
        }

        [Benchmark]
        public Message small() => Message.Parse(_heartbeat, 0, _heartbeat.Length);

        [Benchmark]
        public Message medium() => Message.Parse(_executionReport, 0, _executionReport.Length);

        [Benchmark]
        public Message large() => Message.Parse(_marketDataIncrementalRefresh, 0, _marketDataIncrementalRefresh.Length);
    }
}
