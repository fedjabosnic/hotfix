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
    public class reading_datetimes
    {
        private byte[] _raw;

        [Setup]
        public void Setup()
        {
            _raw = System.Text.Encoding.ASCII.GetBytes("20170327-15:45:13.596");
        }

        // NOTE: We return long here because returning datetime seems to cause allocation
        //       which is possibly an issue in the benchmarking library (investigate)

        [Benchmark(Baseline = true)]
        public long standard() => DateTime.ParseExact(System.Text.Encoding.ASCII.GetString(_raw), "yyyyMMdd-HH:mm:ss.fff", null).Ticks;

        [Benchmark]
        public long hotfix() => _raw.ReadDateTime().Ticks;
    }
}