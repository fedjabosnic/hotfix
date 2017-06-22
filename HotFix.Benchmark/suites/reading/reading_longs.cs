using System;
using System.Text;
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
        private byte[] _raw;

        [Setup]
        public void Setup()
        {
            _raw = System.Text.Encoding.ASCII.GetBytes("123456789");
        }

        [Benchmark(Baseline = true)]
        public long standard() => long.Parse(System.Text.Encoding.ASCII.GetString(_raw));

        [Benchmark]
        public long hotfix() => _raw.GetLong();
    }
}