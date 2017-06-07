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
    public class Longs
    {
        public string Raw { get; set; }

        [Setup]
        public void Setup()
        {
            Raw = "123456789";
        }

        [Benchmark(Baseline = true)]
        public long Standard() => long.Parse(Raw);

        [Benchmark]
        public long Hotfix() => Raw.GetLong();
    }
}