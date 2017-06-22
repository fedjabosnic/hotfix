using System;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using HotFix.Utilities;

namespace HotFix.Benchmark.suites.reading
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000)]
    public class reading_strings
    {
        private byte[] _raw;

        [Setup]
        public void Setup()
        {
            _raw = Encoding.ASCII.GetBytes("123456789");
        }

        [Benchmark(Baseline = true)]
        public string standard() => Encoding.ASCII.GetString(_raw, 3, 3);

        [Benchmark]
        public string hotfix() => _raw.GetString(3, 3);
    }
}