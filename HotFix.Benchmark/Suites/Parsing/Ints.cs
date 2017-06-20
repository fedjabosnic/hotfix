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
    public class Ints
    {
        public byte[] Raw { get; set; }

        [Setup]
        public void Setup()
        {
            Raw = Encoding.ASCII.GetBytes("12345");
        }

        [Benchmark(Baseline = true)]
        public int Standard() => int.Parse(Encoding.ASCII.GetString(Raw));

        [Benchmark]
        public int Hotfix() => Raw.GetInt();
    }
}
