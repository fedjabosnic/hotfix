using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using HotFix.Core;
using HotFix.Transport;
using HotFix.Utilities;

namespace HotFix.Benchmark.suites.session
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000)]
    public class session_receive
    {
        public byte[] Heartbeat;
        public byte[] ExecutionReport;
        public byte[] MarketDataIncrementalRefresh;

        public FIXMessage Message;

        [GlobalSetup]
        public void Setup()
        {
            Message = new FIXMessage();

            Heartbeat =
                System.Text.Encoding.ASCII.GetBytes(
                    ("8=FIX.4.2|9=72|35=0|34=000008059|52=20170531-08:18:01.768|49=SENDER..|56=RECEIVER....." +
                    "|10=202|")
                .Replace("|", "\u0001"));

            ExecutionReport =
                System.Text.Encoding.ASCII.GetBytes(
                    ("8=FIX.4.2|9=352|35=8|34=000008059|52=20170531-08:18:01.768|49=SENDER..|56=RECEIVER.....|20=0|39=2|150=2" +
                    "|17=U201:053117:00000079:B|40=2|55=EUR/CAD|54=1|38=000900000.00|151=000000000.00|14=000900000.00|32=000100000.00|31=00001.503850" +
                    "|6=00001.503940|64=20170602|60=20170531-08:18:01.767|75=20170531|9200=S|9300=0647|9500=00000.000000|37=0804188884|15=EUR|44=00001.504200" +
                    "|10=197|")
                .Replace("|", "\u0001"));

            MarketDataIncrementalRefresh =
                System.Text.Encoding.ASCII.GetBytes(
                    ("8=FIX.4.2|9=963|35=X|34=53677|52=20170525-00:55:16.153|49=SENDER..|56=RECEIVER.....|262=c6424b19-af74-4c17-8266-9c52ca583ad2" +
                    "|268=8" +
                    "|279=2|55=GBP/JPY|269=0|278=1211918436|270=144.808000|271=1000000.000000|110=0.000000|15=GBP|282=290" +
                    "|279=2|55=GBP/JPY|269=0|278=1211918437|270=144.802000|271=2000000.000000|110=0.000000|15=GBP|282=290" +
                    "|279=0|55=GBP/JPY|269=0|278=1211918501|270=144.809000|271=1000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                    "|279=0|55=GBP/JPY|269=0|278=1211918502|270=144.803000|271=2000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                    "|279=2|55=GBP/JPY|269=1|278=1211918438|270=144.826000|271=1000000.000000|110=0.000000|15=GBP|282=290" +
                    "|279=2|55=GBP/JPY|269=1|278=1211918439|270=144.833000|271=2000000.000000|110=0.000000|15=GBP|282=290" +
                    "|279=0|55=GBP/JPY|269=1|278=1211918503|270=144.828000|271=1000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                    "|279=0|55=GBP/JPY|269=1|278=1211918504|270=144.834000|271=2000000.000000|110=0.000000|15=GBP|282=290|735=1|695=5" +
                    "|10=070|")
                .Replace("|", "\u0001"));

            Configuration = new Configuration
            {
                Version = "FIX.4.2",
                Sender = "RECEIVER.....",
                Target = "SENDER..",
                HeartbeatInterval = 5000,
                OutboundSeqNum = 1,
                InboundSeqNum = 1
            };

            Transport = new Canned();
            Session = new Session(Configuration, new RealTimeClock(), Transport, null, 65536, 4096, 1024);
        }

        public Configuration Configuration;
        public Canned Transport;
        public Session Session;

        [Benchmark]
        public void small()
        {
            Transport.Response = Heartbeat;
            Session.State.InboundSeqNum = 8059;

            Session.Receive();
        }

        [Benchmark]
        public void medium()
        {
            Transport.Response = ExecutionReport;
            Session.State.InboundSeqNum = 8059;

            Session.Receive();
        }

        [Benchmark]
        public void large()
        {
            Transport.Response = MarketDataIncrementalRefresh;
            Session.State.InboundSeqNum = 53677;

            Session.Receive();
        }
    }

    public sealed class Canned : ITransport
    {
        public byte[] Response;

        public int Read(byte[] buffer, int offset, int count)
        {
            Buffer.BlockCopy(Response, 0, buffer, offset, Response.Length);

            return Response.Length;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("Message sent from session?");
        }

        public void Dispose()
        {
        }
    }
}
