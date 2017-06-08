using System;
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
        public string Raw { get; set; }

        [Setup]
        public void Setup()
        {
            Raw = "20170327-15:45:13.596";
        }

        // NOTE: We return long here because returning datetime seems to cause allocation
        //       which is possibly an issue in the benchmarking library (investigate)

        [Benchmark(Baseline = true)]
        public long Standard() => DateTime.ParseExact(Raw, "yyyyMMdd-HH:mm:ss.fff", null).Ticks;

        [Benchmark]
        public long Hotfix() => Raw.GetDateTime().Ticks;
    }
}