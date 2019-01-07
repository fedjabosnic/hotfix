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
    public class reading_strings
    {
        public byte[] Raw;

        [GlobalSetup]
        public void Setup()
        {
            Raw = System.Text.Encoding.ASCII.GetBytes("123456789");
        }

        [Benchmark(Baseline = true)]
        public string standard() => System.Text.Encoding.ASCII.GetString(Raw, 3, 3);

        [Benchmark]
        public string hotfix() => Raw.ReadString(3, 3);
    }
}