using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;

namespace Hotfix.Benchmark.Suites
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 100000000)]
    public class Basic
    {
        private byte[] Bytes { get; set; }

        [Setup]
        public void Setup()
        {
            Bytes = new byte[1024000];
        }

        [Benchmark]
        public string Noop() => string.Empty;

        [Benchmark]
        public byte IndexingFirstItem() => Bytes[0];

        [Benchmark]
        public byte IndexingMiddleItem() => Bytes[512000];

        [Benchmark]
        public byte IndexingLastItem() => Bytes[1024000 - 1];
    }
}
