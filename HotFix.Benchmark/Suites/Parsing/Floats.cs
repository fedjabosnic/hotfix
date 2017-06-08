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
    public class Floats
    {
        public string Raw { get; set; }

        [Setup]
        public void Setup()
        {
            Raw = "12345.6789";
        }

        [Benchmark(Baseline = true)]
        public double Standard() => double.Parse(Raw);

        [Benchmark]
        public double Hotfix() => Raw.GetFloat();
    }
}