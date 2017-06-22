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
    public class reading_floats
    {
        private byte[] _raw;

        [Setup]
        public void Setup()
        {
            _raw = System.Text.Encoding.ASCII.GetBytes("12345.6789");
        }

        [Benchmark(Baseline = true)]
        public double standard() => double.Parse(System.Text.Encoding.ASCII.GetString(_raw));

        [Benchmark]
        public double hotfix() => _raw.GetFloat();
    }
}