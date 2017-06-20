using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using HotFix.Utilities;

namespace HotFix.Benchmark.Suites.Parsing
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000)]
    public class DateTimes
    {
        public byte[] Raw { get; set; }

        [Setup]
        public void Setup()
        {
            Raw = Encoding.ASCII.GetBytes("20170327-15:45:13.596");
        }

        // NOTE: We return long here because returning datetime seems to cause allocation
        //       which is possibly an issue in the benchmarking library (investigate)

        [Benchmark(Baseline = true)]
        public long Standard() => DateTime.ParseExact(Encoding.ASCII.GetString(Raw), "yyyyMMdd-HH:mm:ss.fff", null).Ticks;

        [Benchmark]
        public long Hotfix() => Raw.GetDateTime().Ticks;
    }
}