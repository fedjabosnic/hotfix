using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;

namespace HotFix.Benchmark.suites
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 10000)]
    public class basic
    {
        private byte[] Bytes { get; set; }

        [Setup]
        public void Setup()
        {
            Bytes = new byte[1024000];
        }

        [Benchmark]
        public string noop() => string.Empty;

        [Benchmark]
        public byte indexing_first_item() => Bytes[0];

        [Benchmark]
        public byte indexing_middle_item() => Bytes[512000];

        [Benchmark]
        public byte indexing_last_item() => Bytes[1024000 - 1];
    }
}
