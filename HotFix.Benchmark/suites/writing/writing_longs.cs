using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Engines;
using HotFix.Encoding;

namespace HotFix.Benchmark.suites.writing
{
    [MemoryDiagnoser]
    [AllStatisticsColumn]
    [SimpleJob(RunStrategy.Throughput, launchCount: 1, warmupCount: 5, targetCount: 10, invocationCount: 1000)]
    public class writing_longs
    {
        public long Number;
        public byte[] Buffer;

        [GlobalSetup]
        public void Setup()
        {
            Number = 1234567890987654321;
            Buffer = new byte[19];
        }

        [Benchmark(Baseline = true)]
        public int standard()
        {
            var s = Number.ToString();
            return System.Text.Encoding.ASCII.GetBytes(s, 0, s.Length, Buffer, 0);
        }

        [Benchmark]
        public int hotfix() => Buffer.WriteLong(0, Number);
    }
}
