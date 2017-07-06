using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using HotFix.Encoding;

namespace HotFix.Benchmark.suites.reading
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000)]
    public class reading_longs
    {
        public byte[] Raw;

        [Setup]
        public void Setup()
        {
            Raw = System.Text.Encoding.ASCII.GetBytes("123456789");
        }

        [Benchmark(Baseline = true)]
        public long standard() => long.Parse(System.Text.Encoding.ASCII.GetString(Raw));

        [Benchmark]
        public long hotfix() => Raw.ReadLong();
    }
}