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
        public byte[] Raw { get; set; }

        [Setup]
        public void Setup()
        {
            Raw = Encoding.ASCII.GetBytes("123456789");
        }

        [Benchmark(Baseline = true)]
        public string standard() => Encoding.ASCII.GetString(Raw, 3, 3);

        [Benchmark]
        public string hotfix() => Raw.GetString(3, 3);
    }
}